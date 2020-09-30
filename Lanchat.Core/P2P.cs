using System.Collections.Generic;
using System.Net;

namespace Lanchat.Core
{
    public class P2P
    {
        public Server Server { get; }
        public List<Client> ConnectedClients { get; }

        private readonly int port;
        
        public P2P(int port)
        {
            this.port = port;
            ConnectedClients = new List<Client>();
            Server = new Server(IPAddress.Any, port);
        }
        
        public Client Connect(string ipAddress)
        {
            var client =  new Client(ipAddress, port);
            ConnectedClients.Add(client);
            client.ConnectAsync();
            return client;
        }

        public void SendEverywhere(string message)
        {
            Server.Multicast(message);
            ConnectedClients.ForEach(x=> x.SendAsync(message));
        }
    }
}