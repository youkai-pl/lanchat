using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.P2PHandlers;

namespace Lanchat.Core
{
    /// <summary>
    ///     Main class representing network in P2P mode.
    /// </summary>
    public class P2P
    {
        private readonly IConfig config;
        internal readonly List<Node> OutgoingConnections = new();
        private readonly P2PInternalHandlers p2PInternalHandlers;
        private readonly Server server;

        /// <summary>
        ///     Initialize P2P mode.
        /// </summary>
        public P2P(IConfig config)
        {
            this.config = config;
            p2PInternalHandlers = new P2PInternalHandlers(this, this.config);

            server = this.config.UseIPv6
                ? new Server(IPAddress.IPv6Any, this.config.ServerPort, this.config)
                : new Server(IPAddress.Any, this.config.ServerPort, this.config);

            server.SessionCreated += p2PInternalHandlers.OnSessionCreated;
            this.config.PropertyChanged += CoreConfigOnPropertyChanged;
            Broadcasting = new Broadcasting(this.config);
        }

        /// <see cref="Lanchat.Core.Network.Broadcasting" />
        public Broadcasting Broadcasting { get; }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(OutgoingConnections);
                nodes.AddRange(server.IncomingConnections);
                return nodes.Where(x => x.Ready).ToList();
            }
        }

        /// <summary>
        ///     New node connected. After receiving this handlers for node events can be created.
        /// </summary>
        public event EventHandler<Node> ConnectionCreated;

        /// <summary>
        ///     Start server.
        /// </summary>
        public void StartServer()
        {
            server.Start();
        }

        /// <summary>
        ///     Start broadcasting presence.
        /// </summary>
        public void StartBroadcast()
        {
            Broadcasting.Start();
        }

        /// <summary>
        ///     Send message to all nodes.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.Messaging.SendMessage(message));
        }

        /// <summary>
        ///     Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        /// <param name="port">Node port.</param>
        public void Connect(IPAddress ipAddress, int? port = null)
        {
            // Use port from config if it's null
            port ??= config.ServerPort;

            // Throw if node is blocked
            if (config.BlockedAddresses.Contains(ipAddress)) throw new ArgumentException("Node blocked");

            // Throw if node already connected
            if (Nodes.Any(x => x.NetworkElement.Endpoint.Address.Equals(ipAddress)))
                throw new ArgumentException("Already connected to this node");

            // Throw if local address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Contains(ipAddress))
                throw new ArgumentException("Illegal IP address. Cannot connect");

            var client = new Client(ipAddress, port.Value);
            var node = new Node(client, config);
            node.Resolver.Handlers.Add(new NodesListHandler(this, config));
            OutgoingConnections.Add(node);
            node.Connected += p2PInternalHandlers.OnConnected;
            node.Disconnected += p2PInternalHandlers.CloseNode;
            node.CannotConnect += p2PInternalHandlers.CloseNode;
            ConnectionCreated?.Invoke(this, node);
            client.ConnectAsync();
        }

        internal void OnConnectionCreated(Node e)
        {
            ConnectionCreated?.Invoke(this, e);
        }

        private void CoreConfigOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nickname":
                    Nodes.ForEach(x =>
                        x.NetworkOutput.SendUserData(new NicknameUpdate {NewNickname = config.Nickname}));
                    break;

                case "Status":
                    Nodes.ForEach(x => x.NetworkOutput.SendUserData(new StatusUpdate {NewStatus = config.Status}));
                    break;
            }
        }
    }
}