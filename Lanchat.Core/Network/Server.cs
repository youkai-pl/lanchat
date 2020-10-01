using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core.Network
{
    public class Server : TcpServer, INetwork
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<INode>();
        }

        public List<INode> IncomingConnections { get; }

        public void BroadcastMessage(string message)
        {
            IncomingConnections.ForEach(x => x.Output.SendMessage(message));
        }

        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            session.Disconnected += SessionOnSessionDisconnected;
            IncomingConnections.Add(session);
            SessionCreated?.Invoke(this, session);
            return session;
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            var session = (Session) sender;
            IncomingConnections.Remove(session);
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}