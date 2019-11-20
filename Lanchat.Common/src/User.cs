using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    class User
    {
        public User(Guid id, int port)
        {
            Id = id;
            Port = port;
        }

        public string Nickname { get; set; }
        public Guid Id { get; set; }
        public string PublicKey { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
    }
}
