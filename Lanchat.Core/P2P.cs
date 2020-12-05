using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class P2P
    {
        private readonly List<Node> outgoingConnections;
        private readonly Server server;

        /// <summary>
        ///     Initialize p2p mode.
        /// </summary>
        public P2P()
        {
            outgoingConnections = new List<Node>();
            server = new Server(IPAddress.IPv6Any, CoreConfig.ServerPort);
            server.SessionCreated += OnSessionCreated;
            CoreConfig.NicknameChanged += OnNicknameChanged;
        }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(outgoingConnections);
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
        ///     Send message to all nodes.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.NetworkOutput.SendMessage(message));
        }

        /// <summary>
        ///     Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        /// <param name="port">Node port.</param>
        public void Connect(IPAddress ipAddress, int? port = null)
        {
            // Use port from config if it's null
            port ??= CoreConfig.ServerPort;
            
            // Throw if node is blocked
            if (CoreConfig.BlockedAddresses.Contains(ipAddress))
            {
                throw new ArgumentException("Node blocked");
            }
            
            // Throw if node already connected
            if (Nodes.Any(x => x.Endpoint.Address.Equals(ipAddress)))
            {
                throw new ArgumentException("Already connected to this node");
            }

            // Throw if local address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Contains(ipAddress))
            {
                throw new ArgumentException("Illegal IP address. Cannot connect");
            }

            var client = new Client(ipAddress, port.Value);
            var node = new Node(client, false);

            outgoingConnections.Add(node);
            node.Connected += OnConnected;
            node.HardDisconnect += OnHardDisconnect;
            node.CannotConnect += OnCannotConnect;
            node.NetworkInput.NodesListReceived += OnNodesListReceived;
            ConnectionCreated?.Invoke(this, node);
            client.ConnectAsync();
        }

        // Exchange nodes list
        private void OnConnected(object sender, EventArgs e)
        {
            var node = (Node) sender;
            var nodesList = Nodes.Where(x => x.Id != node.Id).Select(x => x.Endpoint.Address).ToList();
            node.NetworkOutput.SendNodesList(nodesList);
        }

        // Send new node event after new session and wait for nodes list
        private void OnSessionCreated(object sender, Node node)
        {
            ConnectionCreated?.Invoke(this, node);
            node.NetworkInput.NodesListReceived += OnNodesListReceived;
            node.Connected += OnConnected;
        }

        // Dispose node after hard disconnection
        private void OnHardDisconnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            outgoingConnections.Remove(node);
            node.Dispose();
        }
        
        // Dispose when connection cannot be established
        private void OnCannotConnect(object sender, EventArgs e)
        {
            var node = (Node) sender;
            outgoingConnections.Remove(node);
            node.Dispose();
        }

        // Try connect to every node from list
        private void OnNodesListReceived(object sender, List<IPAddress> list)
        {
            list.ForEach(x =>
            {
                try
                {
                    Connect(x);
                }
                catch (Exception e)
                {
                    Trace.WriteLine($"Node connection error: {e.Message}");
                }
            });
        }

        // Broadcast new nickname
        private void OnNicknameChanged(object sender, EventArgs e)
        {
            Nodes.ForEach(x => x.NetworkOutput.SendNicknameUpdate(CoreConfig.Nickname));
        }
    }
}