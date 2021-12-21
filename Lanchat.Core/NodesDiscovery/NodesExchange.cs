using System;
using System.Diagnostics;
using Lanchat.Core.Config;
using Lanchat.Core.Network;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.NodesDiscovery
{
    internal class NodesExchange : INodesExchange
    {
        private readonly IConfig config;
        private readonly INodesDatabase nodesDatabase;
        private readonly IP2P network;

        public NodesExchange(IConfig config, INodesDatabase nodesDatabase, IP2P network)
        {
            this.config = config;
            this.nodesDatabase = nodesDatabase;
            this.network = network;
        }

        public void ConnectWithList(INode sender, NodesList nodesList)
        {
            if (!config.ConnectToReceivedList)
            {
                return;
            }

            var nodeInfo = nodesDatabase.GetNodeInfo(sender.Host.Endpoint.Address);
            if (!nodeInfo.Trusted)
            {
                Trace.WriteLine("Node is not trusted. Ignoring list exchange");
                return;
            }

            Trace.WriteLine("Connecting with nodes from list");
            nodesList.ForEach(x =>
            {
                try
                {
                    network.Connect(x);
                }
                catch (ArgumentException)
                { }
            });
        }
    }
}