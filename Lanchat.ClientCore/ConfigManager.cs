using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    public static class ConfigManager
    {
        public static string DownloadsPath { get; set; }
        public static string ConfigPath { get; set; }
        public static string DataPath { get; set; }

        private static JsonSerializerOptions JsonSerializerOptions =>
            new()
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new IpAddressConverter()
                }
            };

        public static Config Load()
        {
            if (DataPath == null)
            {
                SetPaths();
            }

            Config config;

            try
            {
                config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath),
                    JsonSerializerOptions);
            }
            catch (Exception e)
            {
                if (e is not (FileNotFoundException or DirectoryNotFoundException or JsonException)) throw;
                config = CreateNewConfig();
            }

            config ??= CreateNewConfig();

            Save(config);
            config.PropertyChanged += (_, _) => { Save(config); };
            config.BlockedAddresses.CollectionChanged += (_, _) => { Save(config); };
            config.SavedAddresses.CollectionChanged += (_, _) => { Save(config); };
            return config;
        }

        internal static void Save(Config config)
        {
            if (DataPath == null)
            {
                SetPaths();
            }

            try
            {
                var configFileDirectory = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(configFileDirectory))
                    Directory.CreateDirectory(configFileDirectory!);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config, JsonSerializerOptions));
            }
            catch (Exception e)
            {
                if (e is not (DirectoryNotFoundException or UnauthorizedAccessException or ArgumentNullException))
                    throw;
                Trace.WriteLine("Cannot save config");
            }
        }

        private static Config CreateNewConfig()
        {
            return new()
            {
                Fresh = true
            };
        }

        // TODO: Refactor
        private static void SetPaths()
        {
            var xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
            var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");

            if (xdgDataHome != null && xdgConfigHome != null)
            {
                DataPath = xdgDataHome;
                ConfigPath = $"{xdgConfigHome}/config.json";
                DownloadsPath = xdgDataHome;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2";
                ConfigPath = $"{DataPath}/config.json";
                DownloadsPath = Environment.GetEnvironmentVariable("%UserProfile%") + "/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                DataPath = $"{home}/.Lancaht2";
                ConfigPath = $"{DataPath}/config.json";
                DownloadsPath = $"{home}/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                DataPath = $"{home}/Library/Preferences/.Lancaht2";
                ConfigPath = $"{DataPath}/config.json";
                DownloadsPath = $"{home}/Downloads";
            }
        }
    }
}