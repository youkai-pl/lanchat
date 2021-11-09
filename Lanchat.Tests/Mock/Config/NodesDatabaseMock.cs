using System.Net;
using System.Security.Cryptography;
using Lanchat.Core.Config;

namespace Lanchat.Tests.Mock.Config
{
    public class NodesDatabaseMock : INodesDatabase
    {
        private readonly RSA localRsa;
        private readonly RSA remoteRsa;

        public NodesDatabaseMock()
        {
            localRsa = RSA.Create(2048);
            remoteRsa = RSA.Create(2048);
        }

        public string GetLocalNodeInfo()
        {
            return new string(PemEncoding.Write("RSA PRIVATE KEY", localRsa.ExportRSAPrivateKey()));
        }

        public void SaveLocalNodeInfo(string pem)
        {
            localRsa.ImportFromPem(pem);
        }

        NodeInfo INodesDatabase.GetNodeInfo(IPAddress ipAddress)
        {
            var pem = new string(PemEncoding.Write("RSA PUBLIC KEY", remoteRsa.ExportRSAPublicKey()));
            return new NodeInfo
            {
                Nickname = "test",
                Id = 9999,
                IpAddress = IPAddress.Any,
                PublicKey = pem
            };
        }

        public void SaveNodeInfo(IPAddress ipAddress, NodeInfo nodeInfo)
        {
            remoteRsa.ImportFromPem(nodeInfo.PublicKey);

        }
    }
}