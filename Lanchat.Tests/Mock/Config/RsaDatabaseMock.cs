using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;

namespace Lanchat.Tests.Mock.Config
{
    public class RsaDatabaseMock : IRsaDatabase
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;

        public RsaDatabaseMock()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create(2048);
        }

        public string GetLocalPem()
        {
            return new string(PemEncoding.Write("RSA PRIVATE KEY", localRsa.ExportRSAPrivateKey()));
        }

        public void SaveLocalPem(string pem)
        {
            localRsa.ImportFromPem(pem);
        }

        public string GetNodePem(IPAddress ipAddress)
        {
            return new string(PemEncoding.Write("RSA PUBLIC KEY", remoteRsa.ExportRSAPublicKey()));
        }

        public void SaveNodePem(IPAddress ipAddress, string pem)
        {
            remoteRsa.ImportFromPem(pem);
        }
    }
}