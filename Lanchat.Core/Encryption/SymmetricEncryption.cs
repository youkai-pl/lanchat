using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class SymmetricEncryption : ISymmetricEncryption
    {
        private readonly Aes localAes;
        private readonly INodePublicKey nodePublicKey;
        private readonly Aes remoteAes;
        private bool disposed;

        public SymmetricEncryption(INodePublicKey nodePublicKey)
        {
            this.nodePublicKey = nodePublicKey;
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }

        public void Dispose()
        {
            localAes?.Dispose();
            remoteAes?.Dispose();
            nodePublicKey?.Dispose();
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
            return new()
            {
                AesKey = nodePublicKey.Encrypt(localAes.Key),
                AesIv = nodePublicKey.Encrypt(localAes.IV)
            };
        }

        public void ImportKey(KeyInfo keyInfo)
        {
            remoteAes.Key = nodePublicKey.Decrypt(keyInfo.AesKey);
            remoteAes.IV = nodePublicKey.Decrypt(keyInfo.AesIv);
        }

        internal byte[] EncryptBytes(byte[] data)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(SymmetricEncryption));
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
                throw new ObjectDisposedException(nameof(SymmetricEncryption));
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