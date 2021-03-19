using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.ClientCore
{
    public class Config : IConfig
    {
        private static int _port;
        private static int _broadcastPort;
        private static string _nickname;
        private static List<string> _blockedAddresses;
        private static bool _automaticConnecting;
        private static bool _useIPv6;
        private static string _language;
        private int maxMessageLenght;
        private int maxNicknameLenght;

        public Config()
        {
            PropertyChanged += (_, _) => { Save(); };
        }

        public List<string> BlockedAddressesList
        {
            get => _blockedAddresses;
            set { _blockedAddresses = value.Select(x => x.ToString()).ToList(); }
        }

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        [JsonIgnore] public bool Fresh { get; set; }

        public static string ConfigPath { get; private set; }
        public static string DataPath { get; private set; }

        [JsonIgnore]
        public List<IPAddress> BlockedAddresses
        {
            get => BlockedAddressesList.Select(IPAddress.Parse).ToList();
            set { BlockedAddressesList = value.Select(x => x.ToString()).ToList(); }
        }

        public Status Status { get; set; }

        public int ServerPort
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }

        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                OnPropertyChanged(nameof(BroadcastPort));
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public int MaxNicknameLenght
        {
            get => maxNicknameLenght;
            set
            {
                maxNicknameLenght = value;
                OnPropertyChanged(nameof(MaxNicknameLenght));
            }
        }

        public bool AutomaticConnecting
        {
            get => _automaticConnecting;
            set
            {
                _automaticConnecting = value;
                OnPropertyChanged(nameof(AutomaticConnecting));
            }
        }

        public bool UseIPv6
        {
            get => _useIPv6;
            set
            {
                _useIPv6 = value;
                OnPropertyChanged(nameof(UseIPv6));
            }
        }

        public int MaxMessageLenght
        {
            get => maxMessageLenght;
            set
            {
                maxMessageLenght = value;
                OnPropertyChanged(nameof(MaxMessageLenght));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddBlocked(IPAddress ipAddress)
        {
            var ipString = ipAddress.ToString();
            if (BlockedAddressesList.Contains(ipString)) return;
            BlockedAddressesList.Add(ipString);
            BlockedAddresses.Add(ipAddress);
            Save();
        }

        public void RemoveBlocked(IPAddress ipAddress)
        {
            BlockedAddressesList.Remove(ipAddress.ToString());
            BlockedAddresses.Remove(ipAddress);
            Save();
        }

        public static Config Load()
        {
            Config config;
            try
            {
                var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

                if (xdgDataHome != null && xdgConfigHome != null)
                {
                    DataPath = xdgDataHome;
                    ConfigPath = $"{xdgConfigHome}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2";
                    ConfigPath = $"{DataPath}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    DataPath = Environment.GetEnvironmentVariable("HOME") + "/.Lancaht2";
                    ConfigPath = $"{DataPath}/config.json";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    DataPath = Environment.GetEnvironmentVariable("HOME") + "/Library/Preferences/.Lancaht2";
                    ConfigPath = $"{DataPath}/config.json";
                }

                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath), JsonSerializerOptions);
            }
            catch (Exception e)
            {
                if (!(e is FileNotFoundException) &&
                    !(e is DirectoryNotFoundException) &&
                    !(e is JsonException)) throw;

                config = new DefaultConfig().GetDefaultConfig();
                config.Save();
            }

            return config;
        }

        private void Save()
        {
            try
            {
                var configFileDirectory = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(configFileDirectory)) Directory.CreateDirectory(configFileDirectory!);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(this, JsonSerializerOptions));
            }
            catch (Exception e)
            {
                if (!(e is DirectoryNotFoundException) && !(e is UnauthorizedAccessException)) throw;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static JsonSerializerOptions JsonSerializerOptions =>
            new()
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
    }
}