using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Lanchat.Core.Config;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal class NodesControl
    {
        private readonly IConfig config;
        private readonly IContainer container;
        private readonly AddressChecker addressChecker;

        internal NodesControl(IConfig config, IContainer container, AddressChecker addressChecker)
        {
            this.config = config;
            this.container = container;
            this.addressChecker = addressChecker;
            Nodes = new List<INodeInternal>();
        }

        internal List<INodeInternal> Nodes { get; }

        internal INodeInternal CreateNode(IHost host)
        {
            var scope = container.BeginLifetimeScope(b => { b.RegisterInstance(host).As<IHost>(); });
            var node = scope.Resolve<INodeInternal>();
            lock (Nodes)
            {
                Nodes.Add(node);
            }

            node.Connected += OnConnected;
            node.CannotConnect += (sender, args) =>
            {
                CloseNode(sender, args);
                scope.Dispose();
            };

            node.Disconnected += (sender, args) =>
            {
                CloseNode(sender, args);
                scope.Dispose();
            };
            node.Start();
            return node;
        }

        private void CloseNode(object sender, EventArgs e)
        {
            var node = (INodeInternal)sender;
            var id = node.Id;
            var address = node.Host.Endpoint.Address;
            
            lock (Nodes)
            {
                Nodes.Remove(node);
            }
            addressChecker.UnlockAddress(address);
            node.Connected -= OnConnected;
            node.CannotConnect -= CloseNode;
            node.Disconnected -= CloseNode;
            Trace.WriteLine($"Node {id} disposed");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            var node = (INodeInternal)sender;
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