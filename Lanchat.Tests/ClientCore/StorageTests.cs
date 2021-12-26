using System.IO;
using System.Text.Json;
using Lanchat.ClientCore;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    [NonParallelizable]
    public class ConfigManagerTests
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
    }
}