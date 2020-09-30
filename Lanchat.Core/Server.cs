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
            session.SessionDisconnected += SessionOnSessionDisconnected;
            IncomingConnections.Add(session.NetworkOutput);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            IncomingConnections.Remove(session.NetworkOutput);
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}