using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Lanchat.Core.Config;
using Lanchat.Core.Network.Handlers;
using Lanchat.Core.Network.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal class NodesControl
    {
        private readonly IConfig config;
        private readonly P2P network;
        private readonly IContainer container;

        internal NodesControl(IConfig config, P2P network)
        {
            this.config = config;
            this.network = network;
            container = SetupNode.Setup(config);
            Nodes = new List<INode>();
        }

        internal List<INode> Nodes { get; }
        internal event EventHandler<INode> NodeCreated;

        internal Node CreateNode(IHost host)
        {
            var nodeContainer = container.BeginLifetimeScope(b =>
            {
                b.RegisterInstance(host).As<IHost>();
            });
            
            var node = nodeContainer.Resolve<Node>();
            Nodes.Add(node);
            node.Resolver.RegisterHandler(new NodesListHandler(config, network));
            node.Connected += OnConnected;
            node.CannotConnect += CloseNode;
            node.Disconnected += CloseNode;
            NodeCreated?.Invoke(this, node);
            node.Connection.Initialize();
            return node;
        }

        private void CloseNode(object sender, EventArgs e)
        {
            var node = (Node) sender;
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
            var node = (Node) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(Nodes
                .Where(x => x.Id != node.Id)
                .Select(x => x.Host.Endpoint.Address));
            node.Output.SendData(nodesList);

            if (!config.SavedAddresses.Contains(node.Host.Endpoint.Address))
            {
                config.SavedAddresses.Add(node.Host.Endpoint.Address);
            }
        }
    }
}