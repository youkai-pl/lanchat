using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core;
using Lanchat.Core.Api;
using Lanchat.Core.Node;
using Lanchat.Core.NodesDetection;

namespace Lanchat.Tests.Mock
{
    public class P2PMock : IP2P
    {
        public NodesDetector NodesDetection { get; }
        public List<INodePublic> Nodes { get; }
        public Broadcast Broadcast { get; }
        public event EventHandler<INodePublic> NodeCreated;

        public List<IPAddress> Connected { get; } = new();
        
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
    }
}