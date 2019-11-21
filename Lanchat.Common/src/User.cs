using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    class User
    {
        public User(Guid id, int port, IPAddress ip)
        {
            Id = id;
            Port = port;
            Ip = ip;
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
    }
}
