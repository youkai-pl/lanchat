using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Lanchat.Core.Config;
using Lanchat.Core.Network;

namespace Lanchat.Core.NodesDiscovery
{
    internal class NodesExchange
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

        public ObservableCollection<NodesList> ReceivedLists { get; } = new();

        public void ConnectWithList(INode node)
        {
            var list = ReceivedLists.First(x => x.Sender == node);
            list.Addresses.ForEach(x =>
            {
                try
                {
                    network.Connect(x);
                }
                catch (ArgumentException)
                { }
            });
        }

        internal void AddNodesList(INode node, List<IPAddress> addressesList)
        {
            var alreadyConnectedAddresses = network.Nodes.Select(x => x.Host.Endpoint.Address);
            var nodesInDatabase = nodesDatabase.SavedNodes.Select(x => x.IpAddress);
            var alreadyReceivedAddresses = ReceivedLists.SelectMany(x => x.Addresses);

            Trace.WriteLine(alreadyConnectedAddresses.Count());
            Trace.WriteLine(nodesInDatabase.Count());
            Trace.WriteLine(alreadyReceivedAddresses.Count());

            var processedAddressesList = addressesList.Except(alreadyReceivedAddresses).ToList();
            processedAddressesList = addressesList.Except(nodesInDatabase).ToList();
            processedAddressesList = addressesList.Except(alreadyConnectedAddresses).ToList();

            if (processedAddressesList.Count != 0)
            {
                ReceivedLists.Add(new NodesList(node, processedAddressesList));
            }
        }
    }
}