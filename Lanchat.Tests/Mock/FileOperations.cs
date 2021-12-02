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
            Paths.DownloadsDirectory = Paths.RootDirectory;
            Storage.CreateDirectories();
        }

        public static void CleanUp()
        {
            Directory.Delete(Paths.RootDirectory, true);
        }
    }
}