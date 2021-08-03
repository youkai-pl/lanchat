using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class NodeAes : INodeAes
    {
        private readonly IInternalNodeRsa internalNodeRsa;
        private readonly Aes localAes;
        private readonly Aes remoteAes;
        private bool disposed;

        public NodeAes(IInternalNodeRsa internalNodeRsa)
        {
            this.internalNodeRsa = internalNodeRsa;
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }

        public void Dispose()
        {
            localAes?.Dispose();
            remoteAes?.Dispose();
            internalNodeRsa?.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        public string EncryptString(string text)
        {
            var encrypted = EncryptBytes(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(encrypted);
        }

        public string DecryptString(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(DecryptBytes(encryptedBytes));
        }

        public KeyInfo ExportKey()
        {
            return new KeyInfo
            {
                AesKey = internalNodeRsa.Encrypt(localAes.Key),
                AesIv = internalNodeRsa.Encrypt(localAes.IV)
            };
        }

        public void ImportKey(KeyInfo keyInfo)
        {
            remoteAes.Key = internalNodeRsa.Decrypt(keyInfo.AesKey);
            remoteAes.IV = internalNodeRsa.Decrypt(keyInfo.AesIv);
        }

        internal byte[] EncryptBytes(byte[] data)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(NodeAes));
            }

            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, remoteAes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        internal byte[] DecryptBytes(byte[] data)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(NodeAes));
            }

            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, localAes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }
    }
}