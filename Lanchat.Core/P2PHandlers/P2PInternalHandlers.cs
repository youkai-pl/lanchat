using System;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2PHandlers
{
    internal class P2PInternalHandlers
    {
        private readonly IConfig config;
        private readonly P2P p2P;

        internal P2PInternalHandlers(P2P p2P, IConfig config)
        {
            this.p2P = p2P;
            this.config = config;
        }

        internal void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
            p2P.OutgoingConnections.Remove(node);
            node.Dispose();
        }

        internal void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(p2P.Nodes.Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address.ToString()));
            node.NetworkOutput.SendData(nodesList);

            if (config.SavedAddresses.Any(x => !Equals(x, node.NetworkElement.Endpoint.Address)))
            {
                config.SavedAddresses.Add(node.NetworkElement.Endpoint.Address);
            }
        }

        internal void OnSessionCreated(object sender, Node node)
        {
            p2P.OnNodeCreated(node);
            node.Resolver.RegisterHandler(new NodesListHandler(p2P, config));
            node.Connected += OnConnected;
        }
    }
}