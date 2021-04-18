using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock.Encryption
{
    public class SymmetricEncryptionMock : ISymmetricEncryption
    {
        public string EncryptString(string text)
        {
            var charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public string DecryptString(string text)
        {
            var charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public KeyInfo ExportKey()
        {
            return null;
        }

        public void ImportKey(KeyInfo keyInfo)
        { }
    }
}