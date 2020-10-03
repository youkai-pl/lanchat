using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<Node>();
        }

        public List<Node> IncomingConnections { get; }

        public void BroadcastMessage(string message)
        {
            IncomingConnections.ForEach(x => x.SendMessage(message));
        }

        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Node> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            var node = new Node(session);
            session.Disconnected += SessionOnSessionDisconnected;
            IncomingConnections.Add(node);
            SessionCreated?.Invoke(this, node);
            return session;
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            IncomingConnections.Remove(IncomingConnections.Find(x => x.Id == session.Id));
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}