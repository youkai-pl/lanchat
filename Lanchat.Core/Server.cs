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
        /// <summary>
        ///     Create server.
        /// </summary>
        /// <param name="address">Listening IP</param>
        /// <param name="port">Listening port</param>
        public Server(IPAddress address, int port) : base(address, port)
        {
            IncomingConnections = new List<Node>();
        }

        /// <summary>
        ///     List of incoming connections.
        /// </summary>
        public List<Node> IncomingConnections { get; }

        /// <summary>
        ///     Send message for all clients.
        /// </summary>
        /// <param name="message"></param>
        public void BroadcastMessage(string message)
        {
            IncomingConnections.ForEach(x => x.NetworkOutput.SendMessage(message));
        }

        /// <summary>
        ///     Server returned error.
        /// </summary>
        public event EventHandler<SocketError> ServerErrored;

        /// <summary>
        ///     New client connected. After receiving this handlers for client events can be created.
        /// </summary>
        public event EventHandler<Node> SessionCreated;

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);
            var node = new Node(session);
            IncomingConnections.Add(node);
            node.HardDisconnect += OnHardDisconnected;
            SessionCreated?.Invoke(this, node);
            return session;
        }

        protected override void OnError(SocketError error)
        {
            ServerErrored?.Invoke(this, error);
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            IncomingConnections.Remove(node);
            node.Dispose();
        }
    }
}