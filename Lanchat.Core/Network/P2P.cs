using System.Collections.Generic;
using System.Net;

namespace Lanchat.Core.Network
{
    public class P2P : INetwork
    {
        private readonly int port;

        public P2P(int port)
        {
            this.port = port;
            OutgoingConnections = new List<INode>();
            Server = new Server(IPAddress.Any, port);
        }

        public Server Server { get; }
        public List<INode> OutgoingConnections { get; }

        public List<INode> Nodes
        {
            get
            {
                var nodes = new List<INode>();
                nodes.AddRange(OutgoingConnections);
                nodes.AddRange(Server.IncomingConnections);
                return nodes;
            }
        }

        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.Output.SendMessage(message));
        }

        public Client Connect(string ipAddress)
        {
            var client = new Client(ipAddress, port);
            OutgoingConnections.Add(client);
            client.ConnectAsync();
            return client;
        }
    }
}