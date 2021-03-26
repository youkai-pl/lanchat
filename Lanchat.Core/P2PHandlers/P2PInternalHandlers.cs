using System;
using System.Linq;
using System.Threading;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class P2PInternalHandlers
    {
        private readonly IConfig config;
        private readonly P2P network;
        private int reconnect;

        internal P2PInternalHandlers(P2P network, IConfig config)
        {
            this.network = network;
            this.config = config;
        }
        
        internal void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var address = node.NetworkElement.Endpoint.Address;
            network.OutgoingConnections.Remove(node);
            node.Dispose();

            if (reconnect == 3) return;
            Thread.Sleep(1000);
            network.Connect(address);
            reconnect++;
        }

        internal void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            reconnect = 0;
            var nodesList = new NodesList();
            nodesList.AddRange(network.Nodes.Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address.ToString()));
            node.NetworkOutput.SendUserData(nodesList);
        }

        internal void OnSessionCreated(object sender, Node node)
        {
            network.OnConnectionCreated(node);
            node.Resolver.Handlers.Add(new NodesListHandler(network, config));
            node.Connected += OnConnected;
        }
    }
}