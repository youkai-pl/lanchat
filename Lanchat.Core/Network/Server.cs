using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core.Network
{
    internal class Server : TcpServer
    {
        /// <summary>
        ///     Create server.
        /// </summary>
        /// <param name="address">Listening IP</param>
        /// <param name="port">Listening port</param>
        internal Server(IPAddress address, int port) : base(address, port)
        {
            OptionDualMode = true;
            IncomingConnections = new List<Node>();
        }

        /// <summary>
        ///     List of incoming connections.
        /// </summary>
        internal List<Node> IncomingConnections { get; }

        /// <summary>
        ///     New client connected. After receiving this handlers for client events can be created.
        /// </summary>
        internal event EventHandler<Node> SessionCreated;

        protected override void OnStarted()
        {
            Trace.WriteLine($"Server listening on {Endpoint.Port}");
            base.OnStarted();
        }

        protected override TcpSession CreateSession()
        {
            var session = new Session(this);

            session.Connected += (_, _) =>
            {
                if (CoreConfig.BlockedAddresses.Contains(session.Endpoint.Address))
                {
                    Trace.WriteLine($"Connection from {session.Endpoint.Address} blocked");
                    session.Close();
                }
                else
                {
                    var node = new Node(session);
                    IncomingConnections.Add(node);
                    node.HardDisconnect += OnHardDisconnected;
                    SessionCreated?.Invoke(this, node);
                    Trace.WriteLine($"Session for {session.Endpoint.Address} created. Session ID: {session.Id}");
                }
            };

            return session;
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            IncomingConnections.Remove(node);
            node.Dispose();
        }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Server errored: {error}");
        }
    }
}