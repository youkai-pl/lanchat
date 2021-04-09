using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using Lanchat.ClientCore;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    public class ConfigManagerTests
    {
        private IFileSystem fileSystem;
        private ConfigManager configManager;

        [SetUp]
        public void Setup()
        {
            fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {"data", new MockDirectoryData()},
            });
            configManager = new ConfigManager(fileSystem);
        }

        [Test]
        public void CreatingNewConfig()
        {
            var config = configManager.Load();
            Assert.IsTrue(config.Fresh);
        }

        [Test]
        public void ConfigLoading()
        {
            configManager.Save(new Config());
            var config = configManager.Load();
            Assert.IsFalse(config.Fresh);
        }

        [Test]
        public void ConfigSaving()
        {
            var config = ConfigManager.CreateNewConfig();
            config.Nickname = "test";
            var loadedConfig = configManager.Load();
            Assert.AreEqual(config.Nickname, loadedConfig.Nickname);
        }

        [Test]
        public void AddingToBlockList()
        {
            var config = ConfigManager.CreateNewConfig();
            config.BlockedAddresses.Add(IPAddress.Loopback);
            Assert.Contains(IPAddress.Loopback, config.BlockedAddresses);
        }

        [Test]
        public void RemovingFromBlockList()
        {
            var config = ConfigManager.CreateNewConfig();
            config.BlockedAddresses.Add(IPAddress.Loopback);
            config.BlockedAddresses.Remove(IPAddress.Loopback);
            Assert.IsFalse(config.BlockedAddresses.Contains(IPAddress.Loopback));
        }
    }
}