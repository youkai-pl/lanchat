using System;
using Lanchat.Core.Encryption;

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
    }
}