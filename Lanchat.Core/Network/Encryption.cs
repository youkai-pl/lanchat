using System;
using System.IO;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Encryption
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;
        private readonly Aes localAes;
        private readonly Aes remoteAes;

        public Encryption()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create();
            localAes = Aes.Create();
            remoteAes = Aes.Create();
        }
        
        internal string ExportPublicKey()
        {
            var privateKeyBytes = localRsa.ExportRSAPublicKey();
            return Convert.ToBase64String(privateKeyBytes);
        }

        internal void ImportPublicKey(string key)
        {
            remoteRsa.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
        }

        internal KeyInfo ExportAesKey()
        {
            return new KeyInfo
            {
                AesKey = RsaEncrypt(localAes.Key),
                AesIv = RsaEncrypt(localAes.IV),
            };
        }

        internal void ImportAesKey(KeyInfo keyInfo)
        {
            remoteAes.Key = RsaDecrypt(keyInfo.AesKey);
            remoteAes.IV = RsaDecrypt(keyInfo.AesIv);
        }

        internal string Encrypt(string text)
        {
            var encryptor = remoteAes.CreateEncryptor();
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            var encrypted = msEncrypt.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        internal string Decrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            var decryptor = localAes.CreateDecryptor();
            using var msDecrypt = new MemoryStream(encryptedBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var plaintext = srDecrypt.ReadToEnd();
            return plaintext;
        }
        
        private string RsaEncrypt(byte[] bytes)
        {
            var encryptedBytes =  remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        private byte[] RsaDecrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }
    }
}