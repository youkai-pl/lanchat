using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class InternalNodeRsa : INodeRsa, IInternalNodeRsa
    {
        private readonly ILocalPublicKey localPublicKey;
        private readonly RSA remoteRsa;
        private readonly IRsaDatabase rsaDatabase;

        public InternalNodeRsa(IRsaDatabase rsaDatabase, ILocalPublicKey localPublicKey)
        {
            this.rsaDatabase = rsaDatabase;
            this.localPublicKey = localPublicKey;
            remoteRsa = RSA.Create();
        }

        public void Dispose()
        {
            remoteRsa?.Dispose();
            GC.SuppressFinalize(this);
        }

        public PublicKey ExportKey()
        {
            var parameters = localPublicKey.LocalRsa.ExportParameters(false);
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
            return localPublicKey.LocalRsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
        }

        public string PublicPem { get; private set; }
        public KeyStatus KeyStatus { get; private set; }

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

            PublicPem = currentPem;
        }
    }
}