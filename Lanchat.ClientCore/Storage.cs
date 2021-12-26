using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Lanchat.Core.FileSystem;

namespace Lanchat.ClientCore
{
    /// <inheritdoc/>
    public class Storage : IStorage
    {
        /// <inheritdoc cref="Config"/>
        public Config Config { get; set; }

        /// <inheritdoc cref="ThemeModel"/>
        public ThemeModel Theme {get;set;}

        /// <summary>
        ///     Loads config and theme.
        /// </summary>
        public Storage()
        {
            Config = ConfigLoader.Load();
            Theme = ThemeLoader.Load(Config.Theme);
        }

        /// <inheritdoc/>
        public void CatchFileSystemExceptions(Exception e, Action errorHandler = null) => CatchFileSystemExceptionsStatic(e, errorHandler);

        /// <inheritdoc/>
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <inheritdoc/>
        public string GetNewFilePath(string dirtyPath)
        {
            var path = string.Concat(dirtyPath.Split(Path.GetInvalidFileNameChars()));
            var fileName = Path.GetFileNameWithoutExtension(path);
            var fileExt = Path.GetExtension(path);
            path = Path.Combine(Config.ReceivedFilesDirectory, $"received-{fileName}{fileExt}");

            for (var i = 1; ; ++i)
            {
                if (!File.Exists(path))
                {
                    return path;
                }

                path = Path.Combine(Config.ReceivedFilesDirectory, $"received-{fileName}({i}){fileExt}");
            }
        }

        /// <inheritdoc/>
        public long GetFileSize(string path)
        {
            return new FileInfo(path).Length;
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
                CatchFileSystemExceptionsStatic(e);
            }
        }

        internal static void CatchFileSystemExceptionsStatic(Exception e, Action errorHandler = null)
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
            errorHandler?.Invoke();
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
                CatchFileSystemExceptionsStatic(e);
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
    }
}