using System;
using System.Collections.Generic;
using System.Diagnostics;
using Lanchat.Core.Config;
using Lanchat.Core.Network;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.NodesDiscovery
{
    internal class NodesExchange : INodesExchange
    {
        private readonly IConfig config;
        private readonly IP2P network;
        private readonly List<Tuple<INode, NodesList>> awaitingLists = new();

        public NodesExchange(IConfig config, IP2P network)
        {
            this.config = config;
            this.network = network;
        }

        public void ConnectWithList(INode sender, NodesList nodesList)
        {
            if (!config.ConnectToReceivedList)
            {
                return;
            }

            if (!sender.Trusted)
            {
                Trace.WriteLine("Node is not trusted. Ignoring list exchange.");
                awaitingLists.Add(new Tuple<INode, NodesList>(sender, nodesList));
                return;
            }

            Trace.WriteLine("Connecting with nodes from list.");
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

        public void ConnectWithAwaitingList(INode node)
        {
            Trace.WriteLine("Connecting with list");
            var list = awaitingLists.Find(x => x.Item1 == node);
            if (list != null)
            {
                ConnectWithList(node, list.Item2);
                awaitingLists.Remove(list);
            }
        }
    }
}