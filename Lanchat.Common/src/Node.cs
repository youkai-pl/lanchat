using Lanchat.Common.HostLib;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    class Node
    {
        public Node(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
        }

        public void CreateConnection(Handshake handshake)
        {
            Connection = new Client();
            Connection.Connect(Ip, Port, handshake);
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Connection { get; set; }
    }
}
