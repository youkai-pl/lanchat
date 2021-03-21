using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal class Encryptor : IDisposable, IBytesEncryption, IStringEncryption
    {
        private readonly Aes localAes;
        private readonly RSA localRsa;
        private readonly Aes remoteAes;
        private readonly RSA remoteRsa;

        internal Encryptor()
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

        public void Dispose()
        {
            localAes?.Dispose();
            localRsa?.Dispose();
            remoteAes?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }
        
        internal PublicKey ExportPublicKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = parameters.Modulus,
                RsaExponent = parameters.Exponent
            };
        }

        internal KeyInfo ExportAesKey()
        {
            return new()
            {
                AesKey = RsaEncrypt(localAes.Key),
                AesIv = RsaEncrypt(localAes.IV)
            };
        }

        internal void ImportPublicKey(PublicKey publicKey)
        {
            try
            {
                var parameters = new RSAParameters
                {
                    Modulus = publicKey.RsaModulus,
                    Exponent = publicKey.RsaExponent
                };

                remoteRsa.ImportParameters(parameters);

                // Test imported keys
                remoteRsa.Encrypt(new byte[] {0x10}, RSAEncryptionPadding.Pkcs1);
            }
            catch (Exception e)
            {
                throw new InvalidKeyImportException("Cannot import RSA public key", e);
            }
        }

        internal void ImportAesKey(KeyInfo keyInfo)
        {
            try
            {
                remoteAes.Key = RsaDecrypt(keyInfo.AesKey);
                remoteAes.IV = RsaDecrypt(keyInfo.AesIv);
            }
            catch (Exception e)
            {
                throw new InvalidKeyImportException("Cannot import AES key", e);
            }
        }

        private byte[] RsaEncrypt(byte[] bytes)
        {
            var encryptedBytes = remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return encryptedBytes;
        }

        private byte[] RsaDecrypt(byte[] encryptedBytes)
        {
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }
    }
}