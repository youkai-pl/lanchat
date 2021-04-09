using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        public static void Prepare()
        {
            ConfigManager.DataPath = "data";
            ConfigManager.ConfigPath = "data/config.json";
            ConfigManager.DownloadsPath = "data";

            if (Directory.Exists(ConfigManager.DataPath))
            {
                Directory.Delete(ConfigManager.DataPath, true);
            }

            Directory.CreateDirectory(ConfigManager.DataPath);
        }
    }
}