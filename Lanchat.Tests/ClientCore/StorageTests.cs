using System.IO;
using System.Text.Json;
using Lanchat.ClientCore;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    [NonParallelizable]
    public class  StorageTests
    {
        [SetUp]
        public void Setup()
        {
            FileOperations.Prepare();
        }

        [TearDown]
        public void TearDown()
        {
            FileOperations.CleanUp();
        }

        [Test]
        public void CreatingNewConfig()
        {
            var storage = new Storage();
            Assert.IsTrue(storage.Config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            File.WriteAllText(Paths.ConfigFile, JsonSerializer.Serialize(new Config()));
            var storage = new Storage();
            Assert.IsFalse(storage.Config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var storage = new Storage();
            storage.Config.Nickname = "test";
            var reloadedStorage = new Storage();
            Assert.AreEqual(storage.Config.Nickname, reloadedStorage.Config.Nickname);
        }

        [Test]
        public void LoadingInvalidJson()
        {
            File.WriteAllText(Paths.ConfigFile, "not a json");
            var storage = new Storage();
            Assert.IsTrue(storage.Config.Fresh);
        }

        [Test]
        public void ValidNewFilePath()
        {
            const string filename = "test.txt";
            var expectedPath = $"{Paths.DownloadsDirectory}/test.txt";

            var storage = new Storage();
            var path = storage.GetNewFilePath(filename);
            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void NoExtensionFilePath()
        {
            const string filename = "test";
            var expectedPath = $"{Paths.DownloadsDirectory}/test";

            var storage = new Storage();
            var path = storage.GetNewFilePath(filename);
            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void AloneDotInFilePath()
        {
            const string filename = "test.";
            var expectedPath = $"{Paths.DownloadsDirectory}/test";

            var storage = new Storage();
            var path = storage.GetNewFilePath(filename);
            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void MaliciousFilePath()
        {
            const string filename = "../test.txt";
            var expectedPath = $"{Paths.DownloadsDirectory}/test.txt";

            var storage = new Storage();
            var path = storage.GetNewFilePath(filename);
            Assert.AreEqual(expectedPath, path);
        }

        [Test]
        public void OtherMaliciousFilePath()
        {
            const string filename = "/test/test.txt";
            var expectedPath = $"{Paths.DownloadsDirectory}/test.txt";

            var storage = new Storage();
            var path = storage.GetNewFilePath(filename);
            Assert.AreEqual(expectedPath, path);
        }
    }
}