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
        // Private fields
        private readonly Host host;

        private readonly HostEventsHandlers inputs;

        // Properties

        /// <summary>
        /// Self nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Self RSA public key
        /// </summary>
        public string PublicKey { get; set; }

        /// <summary>
        /// UDP broadcast port
        /// </summary>
        public int BroadcastPort { get; set; }

        /// <summary>
        /// TCP host port. Set to -1 for use free ephemeral port.
        /// </summary>
        public int HostPort { get; set; }

        /// <summary>
        /// Self ID. Used for checking udp broadcast duplicates.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// RSA provider
        /// </summary>
        public RsaInstance Rsa { get; set; }

        /// <summary>
        /// All nodes here
        /// </summary>
        public List<Node> NodeList { get; set; }

        /// <summary>
        /// Network output
        /// </summary>
        public Output Output { get; set; }

        /// <summary>
        /// Network input
        /// </summary>
        public Events Events { get; set; }

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
            inputs = new HostEventsHandlers(this);
            host.Events.RecievedBroadcast += inputs.OnReceivedBroadcast;
            host.Events.NodeConnected += inputs.OnNodeConnected;
            host.Events.NodeDisconnected += inputs.OnNodeDisconnected;
            host.Events.ReceivedHandshake += inputs.OnReceivedHandshake;
            host.Events.ReceivedKey += inputs.OnReceivedKey;
            host.Events.RecievedMessage += inputs.OnReceivedMessage;
            host.Events.ChangedNickname += inputs.OnChangedNickname;
            host.Events.ReceivedHeartbeat += inputs.OnReceivedHeartbeat;

            // Create Events instance
            Events = new Events();

            // Create API outputs instance
            Output = new Output(this);
        }

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
    }
}