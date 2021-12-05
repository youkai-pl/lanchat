using System;
using System.Runtime.InteropServices;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Paths used by Lanchat storage.
    ///     If any of the paths has to be overridden set them before initializing other modules.
    /// </summary>
    public static class Paths
    {
        /// <summary>
        ///     Root directory.
        /// </summary>
        public static string RootDirectory { get; set; }

        /// <summary>
        ///     RSA pem files directory.
        /// </summary>
        public static string RsaDirectory => RootDirectory + "/RSA";
        /// <summary>
        ///     Config file path.
        /// </summary>
        public static string ConfigFile => RootDirectory + "/config.json";

        /// <summary>
        ///     Nodes database path.
        /// </summary>
        public static string NodesFile => RootDirectory + "/nodes.json";

        /// <summary>
        ///     File transfer target directory.
        /// </summary>
        public static string DownloadsDirectory { get; set; }

        /// <summary>
        ///     Log files directory.
        /// </summary>
        public static string LogsDirectory => RootDirectory + "/Logs";

        static Paths()
        {
            var xdgDataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
            if (xdgDataHome != null)
            {
                RootDirectory = xdgDataHome;
                DownloadsDirectory = xdgDataHome;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                RootDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Lanchat2";
                DownloadsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                RootDirectory = $"{home}/.Lanchat2";
                DownloadsDirectory = $"{home}/Downloads";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetEnvironmentVariable("HOME");
                RootDirectory = $"{home}/Library/Preferences/.Lanchat2";
                DownloadsDirectory = $"{home}/Downloads";
            }
        }
    }
}