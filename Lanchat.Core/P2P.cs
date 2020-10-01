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
            OutgoingConnections = new List<INetworkElement>();
            Server = new Server(IPAddress.Any, port);
        }

        public Server Server { get; }
        public List<INetworkElement> OutgoingConnections { get; }

        public List<INetworkElement> Nodes
        {
            get
            {
                var nodes = new List<INetworkElement>();
                nodes.AddRange(OutgoingConnections);
                nodes.AddRange(Server.IncomingConnections);
                return nodes;
            }
        }

        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.SendMessage(message));
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