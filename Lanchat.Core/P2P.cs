﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class P2P
    {
        private readonly List<Node> outgoingConnections;
        private readonly int port;
        private readonly Server server;

        /// <summary>
        /// Initialize p2p mode.
        /// </summary>
        /// <param name="port">Server port.</param>
        public P2P(int port)
        {
            this.port = port;
            outgoingConnections = new List<Node>();
            server = new Server(IPAddress.Any, port);
            server.SessionCreated += OnSessionCreated;
        }

        /// <summary>
        /// List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(outgoingConnections);
                nodes.AddRange(server.IncomingConnections);
                return nodes;
            }
        }

        /// <summary>
        /// New node connected. After receiving this handlers for node events can be created.
        /// </summary>
        public event EventHandler<Node> ConnectionCreated;

        /// <summary>
        /// Start server.
        /// </summary>
        public void Start()
        {
            server.Start();
        }

        /// <summary>
        /// Send message to all nodes.
        /// </summary>
        /// <param name="message">Message content.</param>
        public void BroadcastMessage(string message)
        {
            Nodes.ForEach(x => x.NetworkOutput.SendMessage(message));
        }

        /// <summary>
        /// Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        public void Connect(IPAddress ipAddress)
        {
            // Return if node already connected
            if (Nodes.Any(x => x.Endpoint.Address.Equals(ipAddress)))
            {
                return;
            }

            // Return if local address
            var host = Dns.GetHostEntry(Dns.GetHostName());
            if (host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork).Contains(ipAddress))
            {
                return;
            }
            
            var client = new Client(ipAddress, port);
            var node = new Node(client);
            outgoingConnections.Add(node);
            node.Disconnected += OnDisconnectedFromLocal;
            ConnectionCreated?.Invoke(this, node);
            client.ConnectAsync();
        }

        // Create node after new session event.
        private void OnSessionCreated(object sender, Node node)
        {
            node.Connected += OnConnectedFromSession;
            node.NetworkInput.NewNodeInfoReceived += OnNewNodeInfoReceived;
            ConnectionCreated?.Invoke(this, node);
        }

        // Broadcast new node info after successful connection.
        private void OnConnectedFromSession(object sender, EventArgs e)
        {
            var node = (Node) sender;
            Nodes.ForEach(x => x.NetworkOutput.SendNewNodeInfo(node.Endpoint.Address));
        }

        // Remove node from list after disconnection event.
        private void OnDisconnectedFromLocal(object sender, EventArgs e)
        {
            var node = (Node) sender;
            outgoingConnections.Remove(node);
        }
        
        // Connect to new node after receiving node info.
        private void OnNewNodeInfoReceived(object sender, IPAddress address)
        {
            Connect(address);
        }
    }
}