using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal class SymmetricEncryption : ISymmetricEncryption
    {
        private readonly Aes localAes;
        private readonly PublicKeyEncryption publicKeyEncryption;
        private readonly Aes remoteAes;
        private bool disposed;

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
            publicKeyEncryption?.Dispose();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        public byte[] EncryptBytes(byte[] data)
        {
            if (disposed) throw new ObjectDisposedException(nameof(SymmetricEncryption));

            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, remoteAes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public byte[] DecryptBytes(byte[] data)
        {
            if (disposed) throw new ObjectDisposedException(nameof(SymmetricEncryption));

            using var memoryStream = new MemoryStream();
            using var cryptoStream =
                new CryptoStream(memoryStream, localAes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
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

        public void EncryptObject(object data)
        {
            var props = data
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(EncryptAttribute)));

            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, EncryptString(value), null);
            });
        }
        
        public void DecryptObject(object data)
        {
            var props = data
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(EncryptAttribute)));

            props.ForEach(x =>
            {
                var value = x.GetValue(data)?.ToString();
                x.SetValue(data, DecryptString(value), null);
            });
        }
        
        public KeyInfo ExportKey()
        {
            return new()
            {
                AesKey = publicKeyEncryption.Encrypt(localAes.Key),
                AesIv = publicKeyEncryption.Encrypt(localAes.IV)
            };
        }

        public void ImportKey(KeyInfo keyInfo)
        {
            remoteAes.Key = publicKeyEncryption.Decrypt(keyInfo.AesKey);
            remoteAes.IV = publicKeyEncryption.Decrypt(keyInfo.AesIv);
        }
    }
}