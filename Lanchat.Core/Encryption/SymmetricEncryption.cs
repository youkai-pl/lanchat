using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal class SymmetricEncryption : IDisposable
    {
        private readonly Aes localAes;
        private readonly Aes remoteAes;
        private readonly PublicKeyEncryption publicKeyEncryption;

        public SymmetricEncryption(PublicKeyEncryption publicKeyEncryption)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }

        public byte[] Encrypt(byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, remoteAes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public byte[] Decrypt(byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, localAes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public string Encrypt(string text)
        {
            var encrypted = Encrypt(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            var decrypted = Encoding.UTF8.GetString(Decrypt(encryptedBytes));
            return decrypted;
        }

        internal KeyInfo ExportKey()
        {
            return new()
            {
                AesKey = publicKeyEncryption.Encrypt(localAes.Key),
                AesIv = publicKeyEncryption.Encrypt(localAes.IV)
            };
        }

        internal void ImportKey(KeyInfo keyInfo)
        {
            remoteAes.Key = publicKeyEncryption.Decrypt(keyInfo.AesKey);
            remoteAes.IV = publicKeyEncryption.Decrypt(keyInfo.AesIv);
        }

        public void Dispose()
        {
            localAes?.Dispose();
            remoteAes?.Dispose();
        }
    }
}