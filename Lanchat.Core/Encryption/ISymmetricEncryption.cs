using System;

namespace Lanchat.Core.Encryption
{
    internal interface ISymmetricEncryption : IDisposable
    {
        string EncryptString(string text);
        string DecryptString(string text);
    }
}