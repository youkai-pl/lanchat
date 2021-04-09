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
            var config = Storage.LoadConfig();
            Assert.IsTrue(config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            Storage.SaveConfig(new Config());
            var config = Storage.LoadConfig();
            Assert.IsFalse(config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var config = Storage.LoadConfig();
            config.Nickname = "test";
            var loadedConfig = Storage.LoadConfig();
            Assert.AreEqual(config.Nickname, loadedConfig.Nickname);
        }
    }
}