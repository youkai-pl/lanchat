using System;
using System.Security.Cryptography;
using System.Text;
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