using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        public static void Prepare()
        {
            Storage.DataPath = "data";
            Storage.DownloadsPath = "data";

            foreach (var filePath in Directory.GetFiles(Storage.DataPath)) File.Delete(filePath);
        }
    }
}