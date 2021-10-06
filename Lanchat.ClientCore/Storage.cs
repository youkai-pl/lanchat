using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Json;
using Mono.Unix;
using Mono.Unix.Native;

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
        ///     Path of RSA keys database.
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

        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new IpAddressConverter()
            }
        };

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
                config = JsonSerializer.Deserialize<Config>(json, JsonSerializerOptions);
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
            SubscribeEvents(config);
            return config;
        }

        /// <summary>
        ///     Load PEM file.
        /// </summary>
        /// <param name="name">Key ID</param>
        /// <returns>PEM file string</returns>
        public static string ReadPemFile(string name)
        {
            try
            {
                return File.ReadAllText($"{RsaDatabasePath}/{name}.pem");
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
                return null;
            }
        }

        /// <summary>
        ///     Save PEM file.
        /// </summary>
        /// <param name="name">Key ID</param>
        /// <param name="content">PEM file string</param>
        public static void SavePemFile(string name, string content)
        {
            try
            {
                var filePath = $"{RsaDatabasePath}/{name}.pem";
                CreateStorageDirectoryIfNotExists();
                SetPermissions(filePath);
                File.WriteAllText(filePath, content);
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        internal static void SaveConfig(Config config)
        {
            try
            {
                CreateStorageDirectoryIfNotExists();
                SetPermissions(ConfigPath);
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config, JsonSerializerOptions));
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        /// <summary>
        ///     Check is <see cref="DataPath" /> exists and create if it's not.
        /// </summary>
        public static void CreateStorageDirectoryIfNotExists()
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
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        private static void CatchFileSystemExceptions(Exception e)
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

        private static void SubscribeEvents(Config config)
        {
            config.PropertyChanged += (_, _) => { SaveConfig(config); };
            config.BlockedAddresses.CollectionChanged += (_, _) => { SaveConfig(config); };
            config.SavedAddresses.CollectionChanged += (_, _) => { SaveConfig(config); };
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
                DataPath = $"{home}/.Lancaht2";
                DownloadsPath = $"{home}/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                DataPath = $"{home}/Library/Preferences/.Lancaht2";
                DownloadsPath = $"{home}/Downloads";
            }

            RsaDatabasePath = $"{DataPath}/RSA";
        }

        private static void SetPermissions(string filePath)
        {
            File.Create(filePath).Dispose();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return;
            }

            var fileInfo = new UnixFileInfo(filePath)
            {
                FileAccessPermissions = FileAccessPermissions.UserRead | FileAccessPermissions.UserWrite
            };

            fileInfo.Refresh();
        }
    }
}