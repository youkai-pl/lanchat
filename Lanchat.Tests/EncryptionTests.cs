using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class EncryptionTests
    {
        private Encryptor encryptor;

        [SetUp]
        public void Setup()
        {
            encryptor = new Encryptor();
            encryptor.ImportPublicKey(encryptor.ExportPublicKey());
            encryptor.ImportAesKey(encryptor.ExportAesKey());
        }

        [Test]
        public void StringEncryption()
        {
            var testString = "test";
            var encryptedString = encryptor.Encrypt(testString);
            var decryptedString = encryptor.Decrypt(encryptedString);
            Assert.AreEqual(testString, decryptedString);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = encryptor.Encrypt(testBytes);
            var decryptedBytes = encryptor.Decrypt(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void ImportInvalidRsa()
        {
            Assert.Catch<InvalidKeyImportException>(() =>
            {
                encryptor.ImportPublicKey(new PublicKey
                {
                    RsaExponent = new byte[] {0x10},
                    RsaModulus = new byte[] {0x10}
                });
            });
        }

        [Test]
        public void ImportInvalidAes()
        {
            Assert.Catch<InvalidKeyImportException>(() =>
            {
                encryptor.ImportAesKey(new KeyInfo
                {
                    AesIv = new byte[] {0x10},
                    AesKey = new byte[] {0x10}
                });
            });
        }
    }
}