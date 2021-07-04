using System;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption.Models;

namespace Lanchat.Core.Encryption
{
    internal class NodePublicKey : INodePublicKey
    {
        private readonly RSA remoteRsa;
        private readonly IRsaDatabase rsaDatabase;
        private readonly IInternalEncryptionAlerts encryptionAlerts;
        private readonly ILocalPublicKey localPublicKey;

        public NodePublicKey(IRsaDatabase rsaDatabase, IInternalEncryptionAlerts encryptionAlerts,
            ILocalPublicKey localPublicKey)
        {
            this.rsaDatabase = rsaDatabase;
            this.encryptionAlerts = encryptionAlerts;
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
                encryptionAlerts.OnNewKey(currentPem);
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
            else if (savedPem != currentPem)
            {
                Trace.WriteLine("Changed RSA key");
                encryptionAlerts.OnChangedKey(currentPem);
                rsaDatabase.SaveNodePem(ipAddress, currentPem);
            }
        }
    }
}