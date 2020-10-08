using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class P2P
    {
        private readonly int port;

        public P2P(int port)
        {
            this.port = port;
            OutgoingConnections = new List<Node>();
            Server = new Server(IPAddress.Any, port);
            Server.SessionCreated += OnSessionCreated;
        }

        public Server Server { get; }
        public List<Node> OutgoingConnections { get; }

        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(OutgoingConnections);
                nodes.AddRange(Server.IncomingConnections);
                return nodes;
            }
        }

        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.SendMessage(message));
        }

        public Node Connect(string ipAddress)
        {
            var client = new Client(ipAddress, port);
            var node = new Node(client);
            OutgoingConnections.Add(node);
            node.Disconnected += OnDisconnected;
            client.ConnectAsync();
            return node;
        }

        private void OnSessionCreated(object sender, Node node)
        {
            // Send nodes list after successful connection
            node.Connected += (o, args) =>
            {
                node.SendNodesList(Nodes);
            };
        }
        

        private void OnDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            OutgoingConnections.Remove(node);
        }
    }
}