using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock.Encryption
{
    public class SymmetricEncryptionMock : ISymmetricEncryption
    {
        public string EncryptString(string text)
        {
            return text;
        }

        public string DecryptString(string text)
        {
            return text;
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
        {
        }
    }
}