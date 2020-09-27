using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        private readonly List<Node> nodes;
        
        public event EventHandler<SocketError> ServerErrored;
        public event EventHandler<Session> SessionCreated;
        
        // P2P mode
        internal Server(IPAddress address, int port, List<Node> nodes) : base(address, port)
        {
            this.nodes = nodes;
        }
        
        // Server mode
        public Server(IPAddress address, int port) : base(address, port)
        {
            nodes = new List<Node>();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            nodes.Add(new Node(session));
            
            SessionCreated?.Invoke(this, session);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }
    }
}