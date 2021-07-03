using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class PublicKeyEncryption : IPublicKeyEncryption
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;
        private IRsaDatabase rsaDatabase;

        public PublicKeyEncryption(IRsaDatabase rsaDatabase)
        {
            this.rsaDatabase = rsaDatabase;
            try
            {
                localRsa = RSA.Create();
                localRsa.ImportFromPem(rsaDatabase.GetLocalPem());
            }
            catch (ArgumentException)
            {
                localRsa = RSA.Create(2048);
                var pemFile = PemEncoding.Write("RSA PRIVATE KEY", localRsa.ExportRSAPrivateKey());
                rsaDatabase.SaveLocalPem(new string(pemFile));
            }

            remoteRsa = RSA.Create();
        }

        public void Dispose()
        {
            localRsa?.Dispose();
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        public PublicKey ExportKey()
        {
            var parameters = localRsa.ExportParameters(false);
            return new PublicKey
            {
                RsaModulus = parameters.Modulus,
                RsaExponent = parameters.Exponent
            };
        }

        public void ImportKey(PublicKey publicKey, IPAddress remoteIp)
        {
            var parameters = new RSAParameters
            {
                Modulus = publicKey.RsaModulus,
                Exponent = publicKey.RsaExponent
            };


            remoteRsa.ImportParameters(parameters);
            TestKeys();
            CompareWithSaved(remoteIp);
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return remoteRsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        }

        public byte[] Decrypt(byte[] encryptedBytes)
        {
            return localRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }

        private void TestKeys()
        {
            remoteRsa.Encrypt(new byte[] {0x10}, RSAEncryptionPadding.Pkcs1);
        }

        private void CompareWithSaved(IPAddress ipAddress)
        {
            var savedPem = rsaDatabase.GetNodePem(ipAddress);
            var currentPem = new string(PemEncoding.Write("RSA PUBLIC KEY", remoteRsa.ExportRSAPublicKey()));
            if (savedPem == null)
            {
                Trace.WriteLine("New RSA key");
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
            else if (savedPem != currentPem)
            {
                Trace.WriteLine("Changed RSA key");
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
        }
    }
}