using Lanchat.Console.Ui;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Lanchat.Console.ProgramLib
{
    public static class Config
    {
        private static int _BroadcastPort;

        private static int _HostPort;

        private static string _Nickname;

        public static int BroadcastPort
        {
            get => _BroadcastPort;
            set
            {
                _BroadcastPort = value;
                Save();
            }
        }

        public static string Path { get; set; }

        public static int HostPort
        {
            get => _HostPort;
            set
            {
                _HostPort = value;
                Save();
            }
        }

        public static List<IPAddress> Muted { get; set; } = new List<IPAddress>();

        public static string Nickname
        {
            get => _Nickname;
            set
            {
                _Nickname = value;
                Save();
            }
        }

        // Add muted node
        public static void AddMute(IPAddress ip)
        {
            Muted.Add(ip);
            Save();
        }

        // Load config
        public static void Load()
        {
            try
            {
                // Detect OS
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2/";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Path = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2/";
                }

                // Load config to dynamic object
                dynamic json = JsonConvert.DeserializeObject(File.ReadAllText(Path + "config.json"));

                // Try use loaded config
                _Nickname = json.nickname;
                _BroadcastPort = json.broadcastport;
                _HostPort = json.hostport;

                // Convert strings to ip addresses
                List<string> MutedStrings = json.muted.ToObject<List<string>>();
                for (int i = 0; i < MutedStrings.Count; i++)
                {
                    Muted.Add(IPAddress.Parse(MutedStrings[i]));
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"[APP] Config load error ({e.Message})");

                Nickname = "";
                BroadcastPort = 4001;
                HostPort = -1;
                Muted = new List<IPAddress>();
            }
        }

        // Remove muted node
        public static void RemoveMute(IPAddress ip)
        {
            Muted.Remove(ip);
            Save();
        }

        // Save config
        private static void Save()
        {
            // Convert ip addresses to strings
            var MutedStrings = new List<string>();
            for (int i = 0; i < Muted.Count; i++)
            {
                MutedStrings.Add(Muted[i].ToString());
            }

            object json = new
            {
                nickname = Nickname,
                broadcastport = BroadcastPort,
                hostport = HostPort,
                muted = MutedStrings
            };
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + "config.json", JsonConvert.SerializeObject(json, Formatting.Indented));
            }
            catch (Exception e)
            {
                Prompt.CrashScreen(e);
            }
        }
    }
}