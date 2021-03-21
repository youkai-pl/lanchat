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
        private static Config _config;
        public static string ConfigPath { get; private set; }
        public static string DataPath { get; private set; }

        public static Config Load()
        {
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

                _config = JsonSerializer.Deserialize<Config>(File.ReadAllText(ConfigPath), JsonSerializerOptions);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException ||
                    e is DirectoryNotFoundException ||
                    e is JsonException)
                {
                    _config = new DefaultConfig().GetDefaultConfig();
                    Save();
                }
                else
                {
                    throw;
                }
            }

            _config ??= new DefaultConfig().GetDefaultConfig();
            _config.PropertyChanged += (_, _) => { Save(); };
            return _config;
        }

        private static void Save()
        {
            try
            {
                var configFileDirectory = Path.GetDirectoryName(ConfigPath);
                if (!Directory.Exists(configFileDirectory)) Directory.CreateDirectory(configFileDirectory!);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(_config, JsonSerializerOptions));
            }
            catch (Exception e)
            {
                if (e is DirectoryNotFoundException ||
                    e is UnauthorizedAccessException ||
                    e is ArgumentNullException)
                {
                    Trace.WriteLine("Cannot save config");
                }
                else
                {
                    throw;
                }
            }
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