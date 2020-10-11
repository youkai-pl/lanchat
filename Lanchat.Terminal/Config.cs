using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Lanchat.Terminal
{
    public class Config
    {
        private static int _broadcastPort;
        private static int _heartbeatSendTimeout;
        private static int _heartbeatReceiveTimeout;
        private static int _connectionTimeout;
        private static int _hostPort;
        private static string _nickname;

        [JsonConstructor]
        public Config(string nickname, int broadcast, int host, List<string> muted, int heartbeatSend,
            int heartbeatReceive, int connection)
        {
            Nickname = nickname;
            BroadcastPort = broadcast;
            HostPort = host;
            HeartbeatSendTimeout = heartbeatSend;
            HeartbeatReceiveTimeout = heartbeatReceive;
            ConnectionTimeout = connection;
            Muted = muted ?? new List<string>();
        }

        [DefaultValue(4001)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                Save();
            }
        }

        [DefaultValue(3000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HeartbeatSendTimeout
        {
            get => _heartbeatSendTimeout;
            set
            {
                _heartbeatSendTimeout = value;
                Save();
            }
        }

        [DefaultValue(5000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HeartbeatReceiveTimeout
        {
            get => _heartbeatReceiveTimeout;
            set
            {
                _heartbeatReceiveTimeout = value;
                Save();
            }
        }

        [DefaultValue(5000)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int ConnectionTimeout
        {
            get => _connectionTimeout;
            set
            {
                _connectionTimeout = value;
                Save();
            }
        }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int HostPort
        {
            get => _hostPort;
            set
            {
                _hostPort = value;
                Save();
            }
        }

        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public List<string> Muted { get; private set; }

        [DefaultValue("user")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                Save();
            }
        }

        public static string Path { get; private set; }

        public static Config Load()
        {
            try
            {
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

                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(Path + "config.json"));
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) && !(e is DirectoryNotFoundException) &&
                    !(e is JsonSerializationException) && !(e is JsonReaderException))
                {
                    throw;
                }

                Trace.WriteLine("[APP] Config load error");
                return JsonConvert.DeserializeObject<Config>("{}");

            }
        }

        public void AddMute(IPAddress ip)
        {
            if (ip == null)
            {
                return;
            }

            Muted.Add(ip.ToString());
            Save();
        }

        public void RemoveMute(IPAddress ip)
        {
            if (ip == null)
            {
                return;
            }

            Muted.Remove(ip.ToString());
            Save();
        }

        private void Save()
        {
            try
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }

                File.WriteAllText(Path + "config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception e)
            {
                if (!(e is DirectoryNotFoundException) && !(e is UnauthorizedAccessException))
                {
                    throw;
                }

                Trace.WriteLine(e.Message);
            }
        }
    }
}