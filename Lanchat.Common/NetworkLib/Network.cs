using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib;
using Lanchat.Common.HostLib.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    ///  Main class of network lib.
    /// </summary>
    public partial class Network
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
            Rsa = new RsaInstance();

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
            host.Events.RecievedBroadcast += hostHandlers.OnReceivedBroadcast;
            host.Events.NodeConnected += hostHandlers.OnNodeConnected;
            host.Events.NodeDisconnected += hostHandlers.OnNodeDisconnected;
            host.Events.ReceivedHandshake += hostHandlers.OnReceivedHandshake;
            host.Events.ReceivedKey += hostHandlers.OnReceivedKey;
            host.Events.RecievedMessage += hostHandlers.OnReceivedMessage;
            host.Events.ChangedNickname += hostHandlers.OnChangedNickname;
            host.Events.ReceivedHeartbeat += hostHandlers.OnReceivedHeartbeat;

            // Create Events instance
            Events = new Events();

            // Create API outputs instance
            Output = new Output(this);
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
        public Output Output { get; set; }

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
        internal RsaInstance Rsa { get; set; }

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