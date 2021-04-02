using System;
using System.Security.Cryptography;
using Lanchat.Core.Models;

namespace Lanchat.Core.Encryption
{
    internal class PublicKeyEncryption : IDisposable
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;

        internal PublicKeyEncryption()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create();
        }
        
        public void Dispose()
        {
            localRsa?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        internal PublicKey ExportKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = parameters.Modulus,
                RsaExponent = parameters.Exponent
            };
        }
        
        internal void ImportKey(PublicKey publicKey)
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
        
        internal byte[] Encrypt(byte[] bytes)
        {
            var encryptedBytes = remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
            return encryptedBytes;
        }

        internal byte[] Decrypt(byte[] encryptedBytes)
        {
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }
    }
}