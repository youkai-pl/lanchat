using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using Lanchat.Core;

namespace Lanchat.ClientCore
{
    public class Config
    {
        private static int _port = 3645;
        private static int _broadcastPort = 3646;
        private static string _nickname = "user";
        private static List<string> _blockedAddresses = new();

        public List<string> BlockedAddresses
        {
            get => _blockedAddresses;
            set
            {
                _blockedAddresses = value;
                CoreConfig.BlockedAddresses = _blockedAddresses.Select(IPAddress.Parse).ToList();
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                CoreConfig.ServerPort = value;
                Save();
            }
        }

        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                CoreConfig.BroadcastPort = value;
                Save();
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                CoreConfig.Nickname = value;
                Save();
            }
        }

        public static string Path { get; private set; }

        public void AddBlocked(IPAddress ipAddress)
        {
            var ipString = ipAddress.MapToIPv6().ToString();
            if (BlockedAddresses.Contains(ipString)) return;
            BlockedAddresses.Add(ipString);
            CoreConfig.BlockedAddresses.Add(ipAddress);
            Save();
        }

        public void RemoveBlocked(IPAddress ipAddress)
        {
            BlockedAddresses.Remove(ipAddress.MapToIPv6().ToString());
            CoreConfig.BlockedAddresses.Remove(ipAddress);
            Save();
        }

        public static Config Load()
        {
            Config config;
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2/";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    Path = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2/";
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    Path = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2/";

                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(Path + "config.json"));
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException) && !(e is JsonException)) throw;
                Trace.WriteLine("[APP] Config load error");
                config = new Config();
                config.Save();
            }

            return config;
        }

        private void Save()
        {
            try
            {
                if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
                File.WriteAllText(Path + "config.json",
                    JsonSerializer.Serialize(this, new JsonSerializerOptions {WriteIndented = true}));
            }
            catch (Exception e)
            {
                if (!(e is DirectoryNotFoundException) && !(e is UnauthorizedAccessException)) throw;
                Trace.WriteLine(e.Message);
            }
        }
    }
}