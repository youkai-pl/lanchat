using System;
using System.IO;
using Lanchat.ClientCore;

namespace Lanchat.Tests.Mock
{
    public static class FileOperations
    {
        private static Guid _guid;

        public static void Prepare()
        {
            _guid = Guid.NewGuid();
            Paths.RootDirectory = $"test-{_guid}";
            Paths.DownloadsDirectory = $"download={_guid}";
            Directory.CreateDirectory(Paths.RootDirectory);
            Directory.CreateDirectory(Paths.LogsDirectory);
            Directory.CreateDirectory(Paths.DownloadsDirectory);
        }

        public static void CleanUp()
        {
            Directory.Delete(Paths.RootDirectory, true);
            Directory.Delete(Paths.DownloadsDirectory, true);
        }
    }
}