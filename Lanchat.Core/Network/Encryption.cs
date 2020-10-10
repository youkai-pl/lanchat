using System;
using System.Security.Cryptography;
using System.Text;

namespace Lanchat.Core.Network
{
    public class Encryption
    {
        private RSA localRsa;
        private RSA remoteRsa;

        internal string GetPublicKey()
        {
            localRsa = RSA.Create(2048);
            var privateKeyBytes = localRsa.ExportRSAPublicKey();
            return Convert.ToBase64String(privateKeyBytes);
        }

        internal void ImportPublicKey(string key)
        {
            remoteRsa = RSA.Create();
            remoteRsa.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
        }

        internal string Encrypt(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var encryptedBytes =  remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        internal string Decrypt(string text)
        {
            var encryptedBytes = Convert.FromBase64String(text);
            var bytes = localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}