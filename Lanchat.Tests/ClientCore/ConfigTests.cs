using System.Net;
using Lanchat.ClientCore;
using NUnit.Framework;

namespace Lanchat.Tests.ClientCore
{
    public class ConfigTests
    {
        [Test]
        public void AddingToBlockList()
        {
            var config = new Config();
            config.BlockedAddresses.Add(IPAddress.Loopback);
            Assert.Contains(IPAddress.Loopback, config.BlockedAddresses);
        }

        [Test]
        public void RemovingFromBlockList()
        {
            var config = new Config();
            config.BlockedAddresses.Add(IPAddress.Loopback);
            config.BlockedAddresses.Remove(IPAddress.Loopback);
            Assert.IsFalse(config.BlockedAddresses.Contains(IPAddress.Loopback));
        }

        [Test]
        public void AddingToSavedAddressesList()
        {
            var config = new Config();
            config.SavedAddresses.Add(IPAddress.Loopback);
            Assert.Contains(IPAddress.Loopback, config.SavedAddresses);
        }

        [Test]
        public void RemovingSavedAddressesList()
        {
            var config = new Config();
            config.SavedAddresses.Add(IPAddress.Loopback);
            config.SavedAddresses.Remove(IPAddress.Loopback);
            Assert.IsFalse(config.SavedAddresses.Contains(IPAddress.Loopback));
        }
    }
}