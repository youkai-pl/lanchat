using System;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal interface ISymmetricEncryption : IDisposable
    {
        string EncryptString(string text);
        string DecryptString(string text);
        KeyInfo ExportKey();
        void ImportKey(KeyInfo keyInfo);
    }
}