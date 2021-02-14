using Lanchat.Core.Network;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class EncryptionTest
    {
        private Encryption encryption;

        [SetUp]
        public void Setup()
        {
            encryption = new Encryption();
            encryption.ImportPublicKey(encryption.ExportPublicKey());
            encryption.ImportAesKey(encryption.ExportAesKey());
        }

        [Test]
        public void StringEncryption()
        {
            var testString = "test";
            var encryptedString = encryption.Encrypt(testString);
            var decryptedString = encryption.Decrypt(encryptedString);
            Assert.AreEqual(testString, decryptedString);
        }

        [Test]
        public void BytesEncryption()
        {
            var testBytes = new byte[] {0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70};
            var encryptedBytes = encryption.Encrypt(testBytes);
            var decryptedBytes = encryption.Decrypt(encryptedBytes);
            Assert.AreEqual(testBytes, decryptedBytes);
        }
    }
}