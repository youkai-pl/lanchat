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

        public Client Connect(string ipAddress)
        {
            var client = new Client(ipAddress, port);
            OutgoingConnections.Add(client.Node);
            client.ConnectAsync();
            return client;
        }

        public void BroadcastMessage(string message)
        {
            Server.IncomingConnections.ForEach(x => x.SendMessage(message));
            OutgoingConnections.ForEach(x => x.SendMessage(message));
        }
    }
}