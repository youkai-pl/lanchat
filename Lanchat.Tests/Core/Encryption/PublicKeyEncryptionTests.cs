using System;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Encryption;
using Lanchat.Core.Encryption.Models;
using Lanchat.Tests.Mock.Config;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class PublicKeyEncryptionTests
    {
        private NodeRsa nodeRsaTest;

        [SetUp]
        public void Setup()
        {
            nodeRsaTest = new NodeRsa(new RsaDatabaseMock(), new LocalRsa(new RsaDatabaseMock()));
            nodeRsaTest.ImportKey(nodeRsaTest.ExportKey(), IPAddress.Loopback);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = nodeRsaTest.Encrypt(testBytes);
            var decryptedBytes = nodeRsaTest.Decrypt(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void ImportInvalidRsa()
        {
            Assert.Catch<CryptographicException>(() =>
            {
                nodeRsaTest.ImportKey(new PublicKey
                {
                    RsaExponent = new byte[] {0x10},
                    RsaModulus = new byte[] {0x10}
                }, IPAddress.Loopback);
            });
        }

        [Test]
        public void Dispose()
        {
            nodeRsaTest.Dispose();
            Assert.Catch<ObjectDisposedException>(() => { nodeRsaTest.Encrypt(new byte[] {0x10}); });
        }
    }
}