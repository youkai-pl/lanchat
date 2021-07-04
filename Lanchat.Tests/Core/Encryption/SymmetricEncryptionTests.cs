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
        private InternalNodeRsa internalNodeRsa;
        private NodeAes nodeAes;

        [SetUp]
        public void Setup()
        {
            internalNodeRsa = new InternalNodeRsa(new RsaDatabaseMock(), new LocalPublicKey(new RsaDatabaseMock()));
            nodeAes = new NodeAes(internalNodeRsa);
            internalNodeRsa.ImportKey(internalNodeRsa.ExportKey(), IPAddress.Loopback);
            nodeAes.ImportKey(nodeAes.ExportKey());
        }

        [Test]
        public void StringEncryption()
        {
            const string testString = "test";
            var encryptedString = nodeAes.EncryptString(testString);
            var decryptedString = nodeAes.DecryptString(encryptedString);
            Assert.AreEqual(testString, decryptedString);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = nodeAes.EncryptBytes(testBytes);
            var decryptedBytes = nodeAes.DecryptBytes(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void InvalidFormat()
        {
            Assert.Catch<FormatException>(() => { nodeAes.DecryptString("not a base 64"); });
        }

        [Test]
        public void InvalidEncryption()
        {
            Assert.Catch<CryptographicException>(() => { nodeAes.DecryptString("bm90IGVuY3J5cHRlZA=="); });
        }

        [Test]
        public void ImportInvalidAes()
        {
            Assert.Catch<CryptographicException>(() =>
            {
                nodeAes.ImportKey(new KeyInfo
                {
                    AesIv = new byte[] {0x10},
                    AesKey = new byte[] {0x10}
                });
            });
        }

        [Test]
        public void Dispose()
        {
            nodeAes.Dispose();
            Assert.Catch<ObjectDisposedException>(() => { nodeAes.EncryptString("test"); });
            Assert.Catch<ObjectDisposedException>(() => { nodeAes.DecryptString("test"); });
        }
    }
}