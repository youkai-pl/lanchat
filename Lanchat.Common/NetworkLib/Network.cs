using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        // Private fields
        private readonly string nickname;
        private readonly Host host;

        // Properties
        public string PublicKey { get; set; }
        public int BroadcastPort { get; set; }
        public int HostPort { get; set; }
        public Guid Id { get; set; }
        public Cryptography Cryptography { get; set; }
        public List<Node> NodeList { get; set; }
        public ApiOutputs ApiOutputs { get; set; }
        public Events Events { get; set; }

        public Network(int port, string nickname)
        {
            // Initialize RSA provider
            Cryptography = new Cryptography();

            // Initialize node list
            NodeList = new List<Node>();

            // Set properties
            this.nickname = nickname;
            PublicKey = Cryptography.PublicKey;
            BroadcastPort = port;
            Id = Guid.NewGuid();
            HostPort = FreeTcpPort();

            // Create host class
            host = new Host(BroadcastPort);

            // Listen host events
            var handlers = new EventHandlers(this);
            host.RecievedBroadcast += handlers.OnRecievedBroadcast;
            host.NodeConnected += handlers.OnNodeConnected;
            host.NodeDisconnected += handlers.OnNodeDisconnected;
            host.RecievedHandshake += handlers.OnRecievedHandshake;
            host.ReciecedKey += handlers.OnRecievedKey;
            host.RecievedMessage += handlers.OnRecievedMessage;
            host.ChangedNickname += handlers.OnChangedNickname;

            // Create Events instance
            Events = new Events();

            // Create API outputs instance 
            ApiOutputs = new ApiOutputs(this);
        }

        public void Start()
        {
            // Initialize host
            host.StartHost(HostPort);

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