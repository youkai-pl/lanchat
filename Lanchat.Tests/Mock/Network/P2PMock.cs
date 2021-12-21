using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Network;
using Lanchat.Core.NodesDiscovery;
using Lanchat.Tests.Mock.Config;

namespace Lanchat.Tests.Mock.Network
{
    public class P2PMock : IP2P
    {
        public P2PMock()
        {
            LocalRsa = new LocalRsa(new NodesDatabaseMock());
            NodesDetection = new NodesDetection(new ConfigMock());
            Broadcast = new Broadcast(Nodes.Cast<INodeInternal>().ToList());
        }

        public List<IPAddress> Connected { get; } = new();
        public IBroadcast Broadcast { get; }
        public ILocalRsa LocalRsa { get; }
        public NodesDetection NodesDetection { get; }
        public List<INode> Nodes { get; } = new();

        public void Start()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Connect(IPAddress ipAddress, int? port = null)
        {
            if (Connected.Contains(ipAddress))
            {
                throw new ArgumentException("Already connected");
            }

            Connected.Add(ipAddress);
            return new Task<bool>(() => true);
        }

        public Task<bool> Connect(string hostname, int? port = null)
        {
            throw new NotImplementedException();
        }
    }
}