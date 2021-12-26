using System;
using System.Security.Cryptography;
using Lanchat.Core.Filesystem;

namespace Lanchat.Core.Encryption
{
    internal class LocalRsa : ILocalRsa
    {
        public LocalRsa(INodesDatabase nodesDatabase)
        {
            try
            {
                Rsa = RSA.Create();
                Rsa.ImportFromPem(nodesDatabase.GetLocalNodeInfo());
            }
            catch (ArgumentException)
            {
                Rsa = RSA.Create(2048);
                var pemFile = PemEncoding.Write("RSA PRIVATE KEY", Rsa.ExportRSAPrivateKey());
                nodesDatabase.SaveLocalNodeInfo(new string(pemFile));
            }
        }

        public RSA Rsa { get; }
    }
}