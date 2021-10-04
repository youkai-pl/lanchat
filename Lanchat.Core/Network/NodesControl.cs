using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            CheckAddress(host.Endpoint.Address);
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
            var node = (INodeInternal)sender;
            var id = node.Id;
            Nodes.Remove(node);
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
        
        private void CheckAddress(IPAddress ipAddress)
        {
            if (config.BlockedAddresses.Contains(ipAddress))
            {
                throw new ArgumentException("Node blocked");
            }
            
            if (Nodes.Any(x => x.Host.Endpoint.Address.Equals(ipAddress)))
            {
                throw new ArgumentException("Already connected to this node");
            }

            if ((GetLocalAddresses().Any(x => x.Equals(ipAddress)) ||
                 ipAddress.Equals(IPAddress.Loopback) ||
                 ipAddress.Equals(IPAddress.IPv6Loopback))
                && !config.DebugMode)
            {
                throw new ArgumentException("Address belong to local machine");
            }
        }

        private static IEnumerable<IPAddress> GetLocalAddresses()
        {
            try
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            }
            catch (SocketException)
            {
                Trace.WriteLine("Cannot get local addresses.");
                return new[]
                {
                    IPAddress.Loopback
                };
            }
        }
    }
}