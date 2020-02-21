using Lanchat.Common.Cryptography;
using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Lanchat.Common.NetworkLib.Api;
using System.Diagnostics;
using System.Globalization;
using System.Timers;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    ///  Main class of network lib.
    /// </summary>
    public class Network : IDisposable
    {
        private readonly Host host;
        private readonly HostEventsHandlers hostHandlers;
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
            host.Events.ReceivedList += hostHandlers.OnReceivedList;
            host.Events.ChangedNickname += hostHandlers.OnChangedNickname;

            // Create Events instance
            Events = new Events();

            // Create API outputs instance
            Methods = new Methods(this);

            Trace.WriteLine("[NETWORK] Network initialized");
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
                nickname = value;
                ChangeNickname(value);
            }
        }

        /// <summary>
        /// All nodes here.
        /// </summary>
        public List<Node> NodeList { get; }

        /// <summary>
        /// Network API outputs class.
        /// </summary>
        public Methods Methods { get; set; }

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
            host.StartHost(HostPort);
            Events.OnHostStarted(HostPort);
            host.StartBroadcast(new Paperplane(HostPort, Id));
            host.ListenBroadcast();
        }

        // Create node
        internal void CreateNode(Node node, bool manual)
        {
            // Check is node with same ip alredy exist
            if (NodeList.Find(x => x.Ip.Equals(node.Ip)) != null)
            {
                Trace.WriteLine($"[NETWORK] Node already exist ({node.Ip})");
                if (manual)
                {
                    throw new NodeAlreadyExistException();
                }
            }
            else
            {
                node.StateChanged += OnStatusChanged;
                node.HandshakeAccepted += OnHandshakeAccepted;
                node.HandshakeTimeout += OnHandshakeTimeout;

                if (node.Port != 0)
                {
                    node.CreateConnection();
                    node.Client.SendHandshake(new Handshake(Nickname, PublicKey, HostPort));
                    node.Client.SendList(NodeList);
                }
                else
                {
                    Trace.WriteLine($"[NETWORK] One way connection. Waiting for handshake ({node.Ip})");
                }

                NodeList.Add(node);

                Trace.WriteLine($"[NETWORK] Node created successful ({node.Ip}:{node.Port.ToString(CultureInfo.CurrentCulture)})");
            }

            // Handshake accepted event handler
            void OnHandshakeAccepted(object sender, EventArgs e)
            {
                node.Client.SendHandshake(new Handshake(Nickname, PublicKey, HostPort));
                node.Client.SendList(NodeList);
            }

            // Ready change event handler
            void OnStatusChanged(object sender, EventArgs e)
            {
                // Node ready
                if (node.State == Status.Ready)
                {
                    Events.OnNodeConnected(node.Ip, node.Nickname);
                    Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / ready)");
                }

                // Node suspended
                else if (node.State == Status.Suspended)
                {
                    Events.OnNodeSuspended(node.Ip, node.Nickname);
                    Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / suspended)");
                }

                // Node resumed
                else if (node.State == Status.Resumed)
                {
                    node.Client.ResumeConnection(Nickname);
                    node.State = Status.Ready;
                    Events.OnNodeResumed(node.Ip, node.Nickname);
                    Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / resumed)");
                }
            }

            void OnHandshakeTimeout(object o, EventArgs e)
            {
                node.HandshakeTimer.Dispose();

                if (node.Handshake == null)
                {
                    Trace.WriteLine($"[NODE] Handshake timed out {node.Ip}");
                    NodeList.Remove(node);
                    node.Dispose();
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
            NodeList.ForEach(x =>
            {
                if (x.Client != null)
                {
                    x.Client.SendNickname(value);
                }
            });
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose network.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    host.Dispose();
                    Rsa.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose network.
        /// </summary>
        ~Network()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose network.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}