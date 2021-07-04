using System;
using System.Net;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal interface INodePublicKey : IDisposable
    {
        PublicKey ExportKey();
        void ImportKey(PublicKey publicKey, IPAddress remoteIp);
        byte[] Encrypt(byte[] bytes);
        byte[] Decrypt(byte[] encryptedBytes);
    }
}