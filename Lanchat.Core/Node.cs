using System;

namespace Lanchat.Core
{
    public class Node 
    {
        public Guid Id { get; }
        public string Nickname { get; set; }
        public Client Client { get; }
        
        // Server node
        internal Node(Session session)
        {
            Id = session.Id;
        }

        // Client node
        internal Node(Client client)
        {
            Client = client;
            Id = client.Id;
        }
    }
}