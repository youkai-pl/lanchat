using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class EncryptionTests
    {
        private PublicKeyEncryption publicKeyEncryption;

        [SetUp]
        public void Setup()
        {
            publicKeyEncryption = new PublicKeyEncryption();
            publicKeyEncryption.ImportKey(publicKeyEncryption.ExportKey());
            publicKeyEncryption.ImportAesKey(publicKeyEncryption.ExportAesKey());
        }

        [Test]
        public void StringEncryption()
        {
            var testString = "test";
            var encryptedString = publicKeyEncryption.Encrypt(testString);
            var decryptedString = publicKeyEncryption.Decrypt(encryptedString);
            Assert.AreEqual(testString, decryptedString);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = publicKeyEncryption.Encrypt(testBytes);
            var decryptedBytes = publicKeyEncryption.Decrypt(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }

        [Test]
        public void ImportInvalidRsa()
        {
            Assert.Catch<InvalidKeyImportException>(() =>
            {
                publicKeyEncryption.ImportKey(new PublicKey
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
                publicKeyEncryption.ImportAesKey(new KeyInfo
                {
                    AesIv = new byte[] {0x10},
                    AesKey = new byte[] {0x10}
                });
            });
        }
    }
}