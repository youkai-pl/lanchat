using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        public List<NetworkOutput> IncomingConnections { get; }
        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;

        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<NetworkOutput>();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            IncomingConnections.Add(session.NetworkOutput);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}