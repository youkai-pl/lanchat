using System;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class P2PInternalHandlers
    {
        private readonly IConfig config;
        private readonly P2P network;

        internal P2PInternalHandlers(P2P network, IConfig config)
        {
            this.network = network;
            this.config = config;
        }

        internal void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
            network.OutgoingConnections.Remove(node);
            node.Dispose();
        }

        internal void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(network.Nodes.Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address.ToString()));
            node.NetworkOutput.SendUserData(nodesList);
        }

        internal void OnSessionCreated(object sender, Node node)
        {
            network.OnConnectionCreated(node);
            node.Resolver.RegisterHandler(new NodesListHandler(network, config));
            node.Connected += OnConnected;
        }
    }
}