using Lanchat.ClientCore;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    public class ConfigManagerTests
    {
        [SetUp]
        public void Setup()
        {
            FileOperations.Prepare();
        }

        [Test]
        public void CreatingNewConfig()
        {
            var config = ConfigManager.Load();
            Assert.IsTrue(config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            ConfigManager.Save(new Config());
            var config = ConfigManager.Load();
            Assert.IsFalse(config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var config = ConfigManager.Load();
            config.Nickname = "test";
            var loadedConfig = ConfigManager.Load();
            Assert.AreEqual(config.Nickname, loadedConfig.Nickname);
        }
    }
}