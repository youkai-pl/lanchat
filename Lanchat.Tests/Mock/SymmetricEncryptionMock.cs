using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock
{
    public class SymmetricEncryptionMock : ISymmetricEncryption
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public byte[] EncryptBytes(byte[] data)
        {
            return data;
        }

        public byte[] DecryptBytes(byte[] data)
        {
            return data;
        }

        public string EncryptString(string text)
        {
            return text;
        }

        public string DecryptString(string text)
        {
            return text;
        }

        public KeyInfo ExportKey()
        {
            throw new System.NotImplementedException();
        }

        public void ImportKey(KeyInfo keyInfo)
        {
            throw new System.NotImplementedException();
        }

        public void EncryptObject(object data)
        {
        }

        public void DecryptObject(object data)
        {
        }
    }
}