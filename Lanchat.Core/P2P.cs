using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class P2P : INetwork
    {
        private readonly int port;

        public P2P(int port)
        {
            this.port = port;
            OutgoingConnections = new List<Node>();
            Server = new Server(IPAddress.Any, port);
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

        public Client Connect(string ipAddress)
        {
            var client = new Client(ipAddress, port);
            OutgoingConnections.Add(client.Node);
            client.ConnectAsync();
            return client;
        }

        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.SendMessage(message));
        }
    }
}