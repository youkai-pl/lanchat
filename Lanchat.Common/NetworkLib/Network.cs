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
        private readonly Host host;
        private readonly ApiInputs inputs;

        // Properties
        public string Nickname { get; set; }
        public string PublicKey { get; set; }
        public int BroadcastPort { get; set; }
        public int HostPort { get; set; }
        public Guid Id { get; set; }
        public Cryptography Cryptography { get; set; }
        public List<Node> NodeList { get; set; }
        public ApiOutputs Out { get; set; }
        public NetworkEvents Events { get; set; }

        public Network(int port, string nickname)
        {
            // Initialize RSA provider
            Cryptography = new Cryptography();

            // Initialize node list
            NodeList = new List<Node>();

            // Set properties
            Nickname = nickname;
            PublicKey = Cryptography.PublicKey;
            BroadcastPort = port;
            Id = Guid.NewGuid();
            HostPort = FreeTcpPort();

            // Create host class
            host = new Host(BroadcastPort);

            // Listen host events
            inputs = new ApiInputs(this);
            host.RecievedBroadcast += inputs.OnRecievedBroadcast;
            host.NodeConnected += inputs.OnNodeConnected;
            host.NodeDisconnected += inputs.OnNodeDisconnected;
            host.RecievedHandshake += inputs.OnRecievedHandshake;
            host.ReciecedKey += inputs.OnRecievedKey;
            host.RecievedMessage += inputs.OnRecievedMessage;
            host.ChangedNickname += inputs.OnChangedNickname;

            // Create Events instance
            Events = new NetworkEvents();

            // Create API outputs instance 
            Out = new ApiOutputs(this);
        }

        // Start host
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