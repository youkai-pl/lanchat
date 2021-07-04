using System;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Encryption;
using Lanchat.Core.Encryption.Models;
using Lanchat.Tests.Mock.Config;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class SymmetricEncryptionTests
    {
        private NodePublicKey nodePublicKey;
        private SymmetricEncryption symmetricEncryption;

        [SetUp]
        public void Setup()
        {
            nodePublicKey = new NodePublicKey(new RsaDatabaseMock(), new EncryptionAlerts());
            symmetricEncryption = new SymmetricEncryption(nodePublicKey);
            nodePublicKey.ImportKey(nodePublicKey.ExportKey(), IPAddress.Loopback);
            symmetricEncryption.ImportKey(symmetricEncryption.ExportKey());
        }

        [Test]
        public void StringEncryption()
        {
            const string testString = "test";
            var encryptedString = symmetricEncryption.EncryptString(testString);
            var decryptedString = symmetricEncryption.DecryptString(encryptedString);
            Assert.AreEqual(testString, decryptedString);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = symmetricEncryption.EncryptBytes(testBytes);
            var decryptedBytes = symmetricEncryption.DecryptBytes(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void InvalidFormat()
        {
            Assert.Catch<FormatException>(() => { symmetricEncryption.DecryptString("not a base 64"); });
        }

        [Test]
        public void InvalidEncryption()
        {
            Assert.Catch<CryptographicException>(() => { symmetricEncryption.DecryptString("bm90IGVuY3J5cHRlZA=="); });
        }

        [Test]
        public void ImportInvalidAes()
        {
            Assert.Catch<CryptographicException>(() =>
            {
                symmetricEncryption.ImportKey(new KeyInfo
                {
                    AesIv = new byte[] {0x10},
                    AesKey = new byte[] {0x10}
                });
            });
        }

        [Test]
        public void Dispose()
        {
            symmetricEncryption.Dispose();
            Assert.Catch<ObjectDisposedException>(() => { symmetricEncryption.EncryptString("test"); });
            Assert.Catch<ObjectDisposedException>(() => { symmetricEncryption.DecryptString("test"); });
        }
    }
}