using System;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;
        
        public Server(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}