using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.System;

namespace Lanchat.Core
{
    public class P2P
    {
        public readonly IConfig Config;
        internal readonly List<Node> OutgoingConnections = new();
        private readonly P2PInternalHandlers p2PInternalHandlers;
        private readonly Server server;

        /// <summary>
        ///     Initialize p2p mode.
        /// </summary>
        public P2P(IConfig config)
        {
            Config = config;
            p2PInternalHandlers = new P2PInternalHandlers(this, Config);

            server = Config.UseIPv6
                ? new Server(IPAddress.IPv6Any, Config.ServerPort, Config)
                : new Server(IPAddress.Any, Config.ServerPort, Config);

            server.SessionCreated += p2PInternalHandlers.OnSessionCreated;

            Config.PropertyChanged += CoreConfigOnPropertyChanged;

            Broadcasting = new Broadcasting(Config);
        }

        /// <summary>
        ///     Detecting nodes in network.
        /// </summary>
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
        ///     Start broadcasting.
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
            port ??= Config.ServerPort;

            // Throw if node is blocked
            if (Config.BlockedAddresses.Contains(ipAddress)) throw new ArgumentException("Node blocked");

            // Throw if node already connected
            if (Nodes.Any(x => x.NetworkElement.Endpoint.Address.Equals(ipAddress)))
                throw new ArgumentException("Already connected to this node");

            // Throw if local address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Contains(ipAddress))
                throw new ArgumentException("Illegal IP address. Cannot connect");

            var client = new Client(ipAddress, port.Value);
            var node = new Node(client, Config);
            node.NetworkInput.ApiHandlers.Add(new P2PApiHandlers(this, Config));
            OutgoingConnections.Add(node);
            node.Connected += p2PInternalHandlers.OnConnected;
            node.HardDisconnect += p2PInternalHandlers.OnHardDisconnect;
            node.CannotConnect += p2PInternalHandlers.OnCannotConnect;
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
                    Nodes.ForEach(x => x.NetworkOutput.SendUserData(DataTypes.NicknameUpdate, Config.Nickname));
                    break;

                case "Status":
                    Nodes.ForEach(x => x.NetworkOutput.SendUserData(DataTypes.StatusUpdate, Config.Status));
                    break;
            }
        }
    }
}