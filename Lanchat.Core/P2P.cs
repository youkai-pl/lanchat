using System.Collections.Generic;
using System.Net;

namespace Lanchat.Core
{
    public class P2P
    {
        public Server Server { get; }
        public List<Node> Nodes { get; }

        private readonly int port;
        
        public P2P(int port)
        {
            this.port = port;
            Nodes = new List<Node>();
            Server = new Server(IPAddress.Any, port, Nodes);
        }

        public Client Connect(string ipAddress)
        {
            var client =  new Client(ipAddress, port);
            Nodes.Add(new Node(client));
            client.ConnectAsync();
            return client;
        }
    }
}