using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     File system utilities.
    /// </summary>
    public static class Storage
    {
        static Storage()
        {
            SetPaths();
        }

        /// <summary>
        ///     Path of main Lanchat data folder.
        /// </summary>
        public static string DataPath { get; set; }

        /// <summary>
        ///     Path of RSA pem files directory.
        /// </summary>
        public static string RsaDatabasePath { get; set; }

        /// <summary>
        ///     Path of Lanchat config file.
        /// </summary>
        public static string ConfigPath => DataPath + "/config.json";

        /// <summary>
        ///     Path of saved files directory.
        /// </summary>
        public static string DownloadsPath { get; set; }

         /// <summary>
        ///     Path of log files directory.
        /// </summary>
        public static string LogsPath => DataPath + "/Logs";

        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private static readonly ConfigContext ConfigContext = new(JsonSerializerOptions);

        /// <summary>
        ///     Load config from file or create new if it's non present or corrupted.
        /// </summary>
        /// <returns>
        ///     <see cref="Config" />
        /// </returns>
        public static Config LoadConfig()
        {
            Config config;

            try
            {
                var json = File.ReadAllText(ConfigPath);
                config = JsonSerializer.Deserialize(json, ConfigContext.Config);
            }
            catch (JsonException)
            {
                config = new Config { Fresh = true };
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
                config = new Config { Fresh = true };
            }

            SaveConfig(config);
            config!.PropertyChanged += (_, _) => SaveConfig(config);
            return config;
        }

        internal static void SaveConfig(Config config)
        {
            try
            {
                CreateStorageDirectoryIfNotExists();
                SetPermissions(ConfigPath);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config, ConfigContext.Config));
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        internal static void CreateStorageDirectoryIfNotExists()
        {
            try
            {
                if (!Directory.Exists(DataPath))
                {
                    Directory.CreateDirectory(DataPath);
                }

                if (!Directory.Exists(DownloadsPath))
                {
                    Directory.CreateDirectory(DownloadsPath);
                }

                if (!Directory.Exists($"{RsaDatabasePath}"))
                {
                    Directory.CreateDirectory($"{RsaDatabasePath}");
                }

                if (!Directory.Exists(LogsPath))
                {
                    Directory.CreateDirectory(LogsPath);
                }
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        internal static void CatchFileSystemExceptions(Exception e)
        {
            if (e is not (
                DirectoryNotFoundException or
                FileNotFoundException or
                IOException or
                UnauthorizedAccessException))
            {
                throw e;
            }

            Trace.WriteLine("Cannot access file system");
        }

        internal static void SetPermissions(string filePath)
        {
            File.Create(filePath).Dispose();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = "chmod",
                    Arguments = $"0600 {filePath}"
                }
            };

            process.Start();
            process.WaitForExit();
        }

        private static void SetPaths()
        {
            var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
            if (xdgDataHome != null)
            {
                DataPath = xdgDataHome;
                DownloadsPath = xdgDataHome;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                DataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2";
                DownloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                DataPath = $"{home}/.Lanchat2";
                DownloadsPath = $"{home}/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                DataPath = $"{home}/Library/Preferences/.Lanchat2";
                DownloadsPath = $"{home}/Downloads";
            }

            RsaDatabasePath = $"{DataPath}/RSA";
        }
    }
}