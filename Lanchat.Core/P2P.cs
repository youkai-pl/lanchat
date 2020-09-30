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
            OutgoingConnections = new List<NetworkOutput>();
            Server = new Server(IPAddress.Any, port);
        }

        public Server Server { get; }
        public List<NetworkOutput> OutgoingConnections { get; }

        public Client Connect(string ipAddress)
        {
            var client = new Client(ipAddress, port);
            OutgoingConnections.Add(client.NetworkOutput);
            client.ConnectAsync();
            return client;
        }

        public void SendEverywhere(string message)
        {
            Server.IncomingConnections.ForEach(x => x.SendMessage(message));
            OutgoingConnections.ForEach(x => x.SendMessage(message));
        }
    }
}