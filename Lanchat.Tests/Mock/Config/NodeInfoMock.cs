using System.Net;
using Lanchat.Core.Config;

namespace Lanchat.Tests.Mock.Config
{
    public class NodeInfoMock : INodeInfo
    {
        public IPAddress IpAddress { get; set; } = IPAddress.Loopback;
        public int Id { get; set; } = 1;
        public string Nickname { get; set; } = "Test";
        public string PublicKey { get; set; }
        public bool Blocked { get; set; }
    }
}