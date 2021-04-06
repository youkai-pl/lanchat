using System.Security.Cryptography;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Encryption
{
    public class SymmetricEncryptionTests
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