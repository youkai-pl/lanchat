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
        private readonly PublicKeyEncryption publicKeyEncryption;
        private readonly Aes remoteAes;

        internal SymmetricEncryption(PublicKeyEncryption publicKeyEncryption)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }

        public void Dispose()
        {
            localAes?.Dispose();
            remoteAes?.Dispose();
        }

        internal byte[] EncryptBytes(byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, remoteAes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        internal byte[] DecryptBytes(byte[] data)
        {
            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, localAes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        internal string EncryptString(string text)
        {
            var encrypted = EncryptBytes(Encoding.UTF8.GetBytes(text));
            return Convert.ToBase64String(encrypted);
        }

        internal string DecryptString(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(DecryptBytes(encryptedBytes));
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
    }
}