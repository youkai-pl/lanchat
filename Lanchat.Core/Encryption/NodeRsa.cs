using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class NodeRsa : INodeRsa, IInternalNodeRsa
    {
        private readonly ILocalRsa localRsa;
        public RSA Rsa { get; }
        private readonly IRsaDatabase rsaDatabase;

        public NodeRsa(IRsaDatabase rsaDatabase, ILocalRsa localRsa)
        {
            this.rsaDatabase = rsaDatabase;
            this.localRsa = localRsa;
            Rsa = RSA.Create();
        }

        public void Dispose()
        {
            Rsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        public PublicKey ExportKey()
        {
            var parameters = localRsa.Rsa.ExportParameters(false);
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


            Rsa.ImportParameters(parameters);
            TestKeys();
            CompareWithSaved(remoteIp);
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return Rsa.Encrypt(bytes, RSAEncryptionPadding.Pkcs1);
        }

        public byte[] Decrypt(byte[] encryptedBytes)
        {
            return localRsa.Rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }

        public KeyStatus KeyStatus { get; private set; }

        private void TestKeys()
        {
            Rsa.Encrypt(new byte[] {0x10}, RSAEncryptionPadding.Pkcs1);
        }

        private void CompareWithSaved(IPAddress ipAddress)
        {
            var savedPem = rsaDatabase.GetNodePem(ipAddress);
            var currentPem = new string(PemEncoding.Write("RSA PUBLIC KEY", Rsa.ExportRSAPublicKey()));
            if (savedPem == null)
            {
                Trace.WriteLine("New RSA key");
                KeyStatus = KeyStatus.FreshKey;
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
            else if (savedPem != currentPem)
            {
                Trace.WriteLine("Changed RSA key");
                KeyStatus = KeyStatus.ChangedKey;
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
            else
            {
                KeyStatus = KeyStatus.ValidKey;
            }
        }
    }
}