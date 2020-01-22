using Lanchat.Common.Cryptography;
using Lanchat.Common.HostLib;
using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lanchat.Common.NetworkLib.Api;
using System.Diagnostics;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    ///  Main class of network lib.
    /// </summary>
    public class Network
    {
        // Host
        private readonly Host host;

        // Host events handlers
        private readonly HostEventsHandlers hostHandlers;

        // Nickname
        private string nickname;

        /// <summary>
        /// Network constructor.
        /// </summary>
        /// <param name="broadcastPort">UDP broadcast port</param>
        /// <param name="nickname">Self nickname</param>
        /// <param name="hostPort">TCP host port. Set to -1 to use free ephemeral port</param>
        public Network(int broadcastPort, string nickname, int hostPort = -1)
        {
            // Initialize RSA provider
            Rsa = new Rsa();

            // Initialize node list
            NodeList = new List<Node>();

            // Set properties
            Nickname = nickname;
            PublicKey = Rsa.PublicKey;
            BroadcastPort = broadcastPort;
            Id = Guid.NewGuid();

            // Check
            if (hostPort == -1)
            {
                HostPort = FreeTcpPort();
            }
            else
            {
                HostPort = hostPort;
            }

            // Create host class
            host = new Host(BroadcastPort);

            // Listen API events
            hostHandlers = new HostEventsHandlers(this);
            host.Events.NodeConnected += hostHandlers.OnNodeConnected;
            host.Events.NodeDisconnected += hostHandlers.OnNodeDisconnected;
            host.Events.RecievedBroadcast += hostHandlers.OnReceivedBroadcast;
            host.Events.ReceivedHandshake += hostHandlers.OnReceivedHandshake;
            host.Events.ReceivedKey += hostHandlers.OnReceivedKey;
            host.Events.RecievedMessage += hostHandlers.OnReceivedMessage;
            host.Events.ReceivedHeartbeat += hostHandlers.OnReceivedHeartbeat;
            host.Events.ReceivedRequest += hostHandlers.OnReceivedRequest;
            host.Events.ReceivedList += hostHandlers.OnReceivedList;
            host.Events.ChangedNickname += hostHandlers.OnChangedNickname;

            // Create Events instance
            Events = new Events();

            // Create API outputs instance
            Output = new Methods(this);
        }

        /// <summary>
        /// Network API inputs class.
        /// </summary>
        public Events Events { get; set; }

        /// <summary>
        /// Self nickname. On set it sends new nickname to connected client.
        /// </summary>
        public string Nickname
        {
            get => nickname;
            set
            {
                ChangeNickname(value);
            }
        }

        /// <summary>
        /// All nodes here.
        /// </summary>
        public List<Node> NodeList { get; set; }

        /// <summary>
        /// Network API outputs class.
        /// </summary>
        public Methods Output { get; set; }

        /// <summary>
        /// UDP broadcast port.
        /// </summary>
        internal int BroadcastPort { get; set; }

        /// <summary>
        /// TCP host port. Set to -1 for use free ephemeral port.
        /// </summary>
        internal int HostPort { get; set; }

        /// <summary>
        /// Self ID. Used for checking udp broadcast duplicates.
        /// </summary>
        internal Guid Id { get; set; }

        /// <summary>
        /// Self RSA public key.
        /// </summary>
        internal string PublicKey { get; set; }

        /// <summary>
        /// RSA provider.
        /// </summary>
        internal Rsa Rsa { get; set; }

        /// <summary>
        /// Start host, broadcast and listen.
        /// </summary>
        public void Start()
        {
            // Initialize host
            host.StartHost(HostPort);

            // Emit started host event
            Events.OnHostStarted(HostPort);

            // Initialize broadcast
            host.Broadcast(new Paperplane(HostPort, Id));

            // Listen other hosts broadcasts
            host.ListenBroadcast();
        }

        /// <summary>
        /// Manual connect.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="port">Node host port</param>
        public void Connect(IPAddress ip, int port)
        {
            CreateNode(new Node(port, ip), true);
        }

        // Create node
        internal void CreateNode(Node node, bool manual)
        {
            // Create node events handlers
            node.ReadyChanged += OnStatusChanged;

            // Create connection with node
            try
            {
                // Check is node exist
                var checkNode = NodeList.Find(x => x.Ip.Equals(node.Ip));

                // If node doesn't exist create connection in new node
                if (checkNode == null)
                {
                    node.CreateConnection();
                }

                // If node already exist but connection is failed and is created manual
                else if (manual && checkNode.State == Status.Failed)
                {
                    // Dispose old node
                    checkNode.Dispose();

                    // Create connection
                    node.CreateConnection();
                }

                // Else throw error
                else
                {
                    throw new NodeAlreadyExistException();
                }
            }
            catch (Exception ex)
            {
                if (ex is NodeAlreadyExistException)
                {
                    Trace.WriteLine("Node already exist");
                }
                else
                {
                    Trace.WriteLine("Connection failed");
                }

                if (manual)
                {
                    throw new ConnectionFailedException();
                }
                else
                {
                    // Prevent auto connecting to this node.
                    node.State = Status.Failed;
                }
            }

            // Send handshake to node
            node.Client.SendHandshake(new Handshake(Nickname, PublicKey, Id, HostPort));

            // Send list
            node.Client.SendList(NodeList);

            // Add node to list
            NodeList.Add(node);

            // Log
            if (node.State != Status.Failed)
            {
                Trace.WriteLine("New node created");
                Trace.Indent();
                Trace.WriteLine(node.Ip);
                Trace.WriteLine(node.Port.ToString());
                Trace.Unindent();
            }

            // Ready change event
            void OnStatusChanged(object sender, EventArgs e)
            {
                // Node ready
                if (node.State == Status.Ready)
                {
                    Trace.WriteLine($"({node.Ip}) ready");
                    Events.OnNodeConnected(node.Ip, node.Nickname);
                }

                // Node suspended
                else if (node.State == Status.Suspended)
                {
                    Trace.WriteLine($"({node.Ip}) suspended");
                    Events.OnNodeSuspended(node.Ip, node.Nickname);
                }

                // Node resumed
                else if (node.State == Status.Resumed)
                {
                    Trace.WriteLine($"({node.Ip}) resumed");
                    node.Client.ResumeConnection();
                    node.State = Status.Ready;
                    Events.OnNodeResumed(node.Ip, node.Nickname);
                }
            }
        }

        // Find free tcp port
        private static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        // Change nickname
        private void ChangeNickname(string value)
        {
            nickname = value;
            NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.SendNickname(nickname);
                }
            });
        }
    }
}