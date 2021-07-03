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

        internal NodesControl(IConfig config, IContainer container)
        {
            this.config = config;
            this.container = container;
            Nodes = new List<INodeInternal>();
        }

        internal List<INodeInternal> Nodes { get; }

        internal INodeInternal CreateNode(IHost host)
        {
            var scope = container.BeginLifetimeScope(b => { b.RegisterInstance(host).As<IHost>(); });
            var node = scope.Resolve<INodeInternal>();
            Nodes.Add(node);
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
            var node = (INodeInternal) sender;
            var id = node.Id;
            Nodes.Remove(node);
            node.Connected -= OnConnected;
            node.CannotConnect -= CloseNode;
            node.Disconnected -= CloseNode;
            Trace.WriteLine($"Node {id} disposed");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            var node = (INodeInternal) sender;
            var nodesList = new NodesList();
            nodesList.AddRange(Nodes
                .Where(x => x.Id != node.Id)
                .Select(x => x.Host.Endpoint.Address));
            node.Output.SendData(nodesList);

            var savedNode = config.SavedNodes.FirstOrDefault(x => Equals(x.IpAddress, node.Host.Endpoint.Address));
            if (savedNode == null)
            {
                SaveNodeInfo(node);
                Trace.WriteLine("New RSA");
            }
            else
            {
                if (savedNode.PublicKey == Convert.ToBase64String(node.PublicKeyEncryption.GetRemotePublicKey()))
                {
                    return;
                }

                config.SavedNodes.Remove(savedNode);
                SaveNodeInfo(node);
                Trace.WriteLine("Changed RSA");
            }
        }

        private void SaveNodeInfo(INodeInternal node)
        {
            config.SavedNodes.Add(new SavedNode
            {
                IpAddress = node.Host.Endpoint.Address,
                PublicKey = Convert.ToBase64String(node.PublicKeyEncryption.GetRemotePublicKey())
            });
        }
    }
}