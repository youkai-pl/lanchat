using System;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal interface ISymmetricEncryption : IDisposable
    {
        byte[] EncryptBytes(byte[] data);
        byte[] DecryptBytes(byte[] data);
        string EncryptString(string text);
        string DecryptString(string text);
        KeyInfo ExportKey();
        void ImportKey(KeyInfo keyInfo);
        void EncryptObject(object data);
        void DecryptObject(object data);
    }
}