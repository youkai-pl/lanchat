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
            IncomingConnections.ForEach(x => x.NetworkIO.SendMessage(message));
        }

        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Node> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            var node = new Node(session);
            IncomingConnections.Add(node);
            node.Disconnected += OnDisconnected;
            SessionCreated?.Invoke(this, node);
            return session;
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            IncomingConnections.Remove(node);
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}