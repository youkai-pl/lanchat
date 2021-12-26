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
            var config = ConfigLoader.LoadConfig();
            Assert.IsTrue(config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            ConfigLoader.SaveFile(JsonSerializer.Serialize(new Config()), Paths.ConfigFile);
            var config = ConfigLoader.LoadConfig();
            Assert.IsFalse(config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var config = ConfigLoader.LoadConfig();
            config.Nickname = "test";
            var loadedConfig = ConfigLoader.LoadConfig();
            Assert.AreEqual(config.Nickname, loadedConfig.Nickname);
        }

        [Test]
        public void LoadingInvalidJson()
        {
            File.WriteAllText(Paths.ConfigFile, "not a json");
            var config = ConfigLoader.LoadConfig();
            Assert.IsTrue(config.Fresh);
        }
    }
}