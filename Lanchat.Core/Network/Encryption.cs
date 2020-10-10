using System;
using System.Security.Cryptography;

namespace Lanchat.Core.Network
{
    public class Encryption
    {
        private RSA rsa;

        public Encryption()
        {
        }

        internal string GetPublicKey()
        {
            rsa = RSA.Create(2048);
            var privateKeyBytes = rsa.ExportRSAPublicKey();
            return Convert.ToBase64String(privateKeyBytes);
        }

        internal void ImportPublicKey(string key)
        {
            rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(key), out _);
        }
    }
}