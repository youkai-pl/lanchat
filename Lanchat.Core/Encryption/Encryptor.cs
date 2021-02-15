using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    public class Encryptor : IDisposable, IBytesEncryption, IStringEncryption
    {
        private readonly Aes localAes;
        private readonly RSA localRsa;
        private readonly Aes remoteAes;
        private readonly RSA remoteRsa;

        public Encryptor()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create();
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

        public void Dispose()
        {
            localAes?.Dispose();
            localRsa?.Dispose();
            remoteAes?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
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

        internal PublicKey ExportPublicKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = Convert.ToBase64String(parameters.Modulus),
                RsaExponent = Convert.ToBase64String(parameters.Exponent)
            };
        }

        internal void ImportPublicKey(PublicKey publicKey)
        {
            var parameters = new RSAParameters
            {
                Modulus = Convert.FromBase64String(publicKey.RsaModulus),
                Exponent = Convert.FromBase64String(publicKey.RsaExponent)
            };
            remoteRsa.ImportParameters(parameters);
        }

        internal KeyInfo ExportAesKey()
        {
            return new()
            {
                AesKey = RsaEncrypt(localAes.Key),
                AesIv = RsaEncrypt(localAes.IV)
            };
        }

        internal void ImportAesKey(KeyInfo keyInfo)
        {
            remoteAes.Key = RsaDecrypt(keyInfo.AesKey);
            remoteAes.IV = RsaDecrypt(keyInfo.AesIv);
        }

        private string RsaEncrypt(byte[] bytes)
        {
            var encryptedBytes = remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        private byte[] RsaDecrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }
    }
}