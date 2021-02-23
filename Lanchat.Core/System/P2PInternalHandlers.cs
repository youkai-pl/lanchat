using System;
using System.Linq;
using Lanchat.Core.Models;

namespace Lanchat.Core.System
{
    internal class P2PInternalHandlers
    {
        private readonly P2P network;

        internal P2PInternalHandlers(P2P network)
        {
            this.network = network;
        }

        // Dispose node after hard disconnection
        internal void OnHardDisconnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            network.OutgoingConnections.Remove(node);
            node.Dispose();
        }

        // Dispose when connection cannot be established
        internal void OnCannotConnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            network.OutgoingConnections.Remove(node);
            node.Dispose();
        }

        // Exchange nodes list
        internal void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = network.Nodes.Where(x => x.Id != node.Id).Select(x => x.NetworkElement.Endpoint.Address)
                .ToList();
            var stringList = nodesList.Select(x => x.ToString());
            node.NetworkOutput.SendUserData(DataTypes.NodesList, stringList);
        }

        // Send new node event after new session and wait for nodes list
        internal void OnSessionCreated(object sender, Node node)
        {
            network.OnConnectionCreated(node);
            node.NetworkInput.ApiHandlers.Add(new P2PApiHandlers(network));
            node.Connected += OnConnected;
        }
    }
}