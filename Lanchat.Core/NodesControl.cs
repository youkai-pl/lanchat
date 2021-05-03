using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Config;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.Node;

namespace Lanchat.Core
{
    internal class NodesControl
    {
        private readonly IConfig config;
        private readonly P2P network;

        internal NodesControl(IConfig config, P2P network)
        {
            this.config = config;
            this.network = network;
            Nodes = new List<LocalNode>();
        }

        internal List<LocalNode> Nodes { get; }
        internal event EventHandler<LocalNode> NodeCreated;

        internal LocalNode CreateNode(INetworkElement networkElement)
        {
            var node = new LocalNode(networkElement, config);
            Nodes.Add(node);
            node.Resolver.RegisterHandler(new NodesListHandler(config, network));
            node.Connected += OnConnected;
            node.CannotConnect += CloseNode;
            node.Disconnected += CloseNode;
            NodeCreated?.Invoke(this, node);
            return node;
        }

        private void CloseNode(object sender, EventArgs e)
        {
            var node = (LocalNode) sender;
            var id = node.Id;
            Nodes.Remove(node);
            node.Connected -= OnConnected;
            node.CannotConnect -= CloseNode;
            node.Disconnected -= CloseNode;
            node.Dispose();
            Trace.WriteLine($"Node {id} disposed");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            var node = (LocalNode) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(Nodes
                .Where(x => x.Id != node.Id)
                .Select(x => x.NetworkElement.Endpoint.Address));
            node.Output.SendData(nodesList);

            if (!config.SavedAddresses.Contains(node.NetworkElement.Endpoint.Address))
            {
                config.SavedAddresses.Add(node.NetworkElement.Endpoint.Address);
            }
        }
    }
}