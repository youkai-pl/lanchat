using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer, INetwork
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<Node>();
        }

        public List<Node> IncomingConnections { get; }
        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            session.Disconnected += SessionOnSessionDisconnected;
            IncomingConnections.Add(session.Node);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            IncomingConnections.Remove(session.Node);
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }

        public void BroadcastMessage(string message)
        {
            IncomingConnections.ForEach(x => x.SendMessage(message));
        }
    }
}