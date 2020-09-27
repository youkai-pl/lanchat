using System;
using System.Collections.Generic;
using System.Linq;
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
            Server = new Server(IPAddress.Any, port);
            
            Server.SessionCreated += OnSessionCreated;
        }

        private void OnSessionCreated(object sender, Session session)
        {
            session.SessionDisconnected += OnSessionDisconnected;
            Nodes.Add(new Node(session));
        }

        private void OnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            var node = Nodes.First(x => x.Id == session.Id);
            Nodes.Remove(node);
        }

        public Client Connect(string ipAddress)
        {
            var client =  new Client(ipAddress, port);
            Nodes.Add(new Node(client));
            client.ConnectAsync();
            return client;
        }

        public void SendEverywhere(string message)
        {
            Server.Multicast(message);
            foreach (var node in Nodes.Where(node => node.Client != null))
            {
                node.Client.SendAsync(message);
            }
        }
    }
}