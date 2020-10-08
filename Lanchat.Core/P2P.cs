using System;
using System.Collections.Generic;
using System.Linq;
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

        public event EventHandler<Node> ConnectionCreated;

        public void Start()
        {
            Server.Start();
        }

        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.SendMessage(message));
        }

        public void Connect(IPAddress ipAddress)
        {
            if (Nodes.Any(x => x.Endpoint.Address.Equals(ipAddress)))
            {
                return;
            }
            
            var client = new Client(ipAddress, port);
            var node = new Node(client);
            OutgoingConnections.Add(node);
            node.Disconnected += OnDisconnected;
            node.NodesListReceived += OnNodesListReceived;
            client.ConnectAsync();

            ConnectionCreated?.Invoke(this, node);
        }

        private void OnSessionCreated(object sender, Node node)
        {
            ConnectionCreated?.Invoke(this, node);
            node.Connected += (o, args) => { node.SendNodesList(Nodes); };
        }

        private void OnNodesListReceived(object sender, List<IPAddress> list)
        {
            list.ForEach(Connect);
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            OutgoingConnections.Remove(node);
        }
    }
}