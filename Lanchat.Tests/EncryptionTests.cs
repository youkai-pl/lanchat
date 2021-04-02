using System.Security.Cryptography;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class EncryptionTests
    {
        private PublicKeyEncryption publicKeyEncryption;
        private SymmetricEncryption symmetricEncryption;

        [SetUp]
        public void Setup()
        {
            publicKeyEncryption = new PublicKeyEncryption();
            symmetricEncryption = new SymmetricEncryption(publicKeyEncryption);
            publicKeyEncryption.ImportKey(publicKeyEncryption.ExportKey());
            symmetricEncryption.ImportKey(symmetricEncryption.ExportKey());
        }

        [Test]
        public void StringEncryption()
        {
            var testString = "test";
            var encryptedString = symmetricEncryption.Encrypt(testString);
            var decryptedString = symmetricEncryption.Decrypt(encryptedString);
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
            Assert.Catch<CryptographicException>(() =>
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
            Assert.Catch<CryptographicException>(() =>
            {
                symmetricEncryption.ImportKey(new KeyInfo
                {
                    AesIv = new byte[] {0x10},
                    AesKey = new byte[] {0x10}
                });
            });
        }
    }
}