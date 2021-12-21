using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Handles file system operations.
    /// </summary>
    /// <remarks>
    /// <para>
    ///     Currently the only public method is <see cref="LoadConfig"/>.
    /// </para>
    /// <para>
    ///     <see cref="NodesDatabase"/> also uses filesystem but it's loaded by constructor.
    ///     To change file paths use <see cref="Paths"/>.
    /// </para>
    /// </remarks>
    public static class Storage
    {
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private static readonly ConfigContext ConfigContext = new(JsonSerializerOptions);
        private static readonly ThemeContext ThemeContext = new(JsonSerializerOptions);

        /// <summary>
        ///     Load config from json file.
        /// </summary>
        /// <remarks>
        ///     If config file is not present or cannot be parsed new file will be created.
        /// </remarks>
        /// <returns>
        ///     <see cref="Config" />
        /// </returns>
        public static Config LoadConfig()
        {
            Config config;

            try
            {
                var json = File.ReadAllText(Paths.ConfigFile);
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
            config.PropertyChanged += (_, _) => SaveConfig(config);
            return config;
        }

        public static Theme LoadTheme(string themeName)
        {
            try
            {
                var json = File.ReadAllText(Path.Combine(Paths.ThemesDirectory, $"{themeName}.json"));
                return JsonSerializer.Deserialize(json, ThemeContext.Theme);
            }
            catch (JsonException)
            {
                SaveDefaultTheme();
                return new Theme();
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
                SaveDefaultTheme();
                return new Theme();
            }
        }

        internal static void SaveFile(string content, string path)
        {
            try
            {
                EnsureDirectories();
                SetPermissions(path);
                File.WriteAllText(path, content);
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

        private static void EnsureDirectories()
        {
            try
            {
                if (!Directory.Exists(Paths.RootDirectory))
                {
                    Directory.CreateDirectory(Paths.RootDirectory);
                }

                if (!Directory.Exists($"{Paths.RsaDirectory}"))
                {
                    Directory.CreateDirectory($"{Paths.RsaDirectory}");
                }

                if (!Directory.Exists(Paths.LogsDirectory))
                {
                    Directory.CreateDirectory(Paths.LogsDirectory);
                }
                if (!Directory.Exists(Paths.ThemesDirectory))
                {
                    Directory.CreateDirectory(Paths.ThemesDirectory);
                }
            }
            catch (Exception e)
            {
                CatchFileSystemExceptions(e);
            }
        }

        private static void SetPermissions(string filePath)
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

        private static void SaveConfig(Config config)
        {
            var json = JsonSerializer.Serialize(config, ConfigContext.Config);
            SaveFile(json, Paths.ConfigFile);
        }

        private static void SaveDefaultTheme()
        {
            var json = JsonSerializer.Serialize(new Theme(), ThemeContext.Theme);
            SaveFile(json, Path.Combine(Paths.ThemesDirectory, "default.json"));
        }
    }
}