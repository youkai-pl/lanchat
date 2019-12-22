using Lanchat.Common.CryptographyLib;
using Lanchat.Common.HostLib;
using Lanchat.Common.HostLib.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        // Private fields
        private Host host;
        private HostEventsHandlers inputs;

        // Properties
        public string Nickname { get; set; }
        public string PublicKey { get; set; }
        public int BroadcastPort { get; set; }
        public int HostPort { get; set; }
        public Guid Id { get; set; }
        public RsaInstance Rsa { get; set; }
        public List<Node> NodeList { get; set; }
        public ApiMethods Out { get; set; }
        public NetworkEvents Events { get; set; }

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
            host.Events.ReceivedHeartbeat += inputs.OnRececeivedHeartbeat;

            // Create Events instance
            Events = new NetworkEvents();

            // Create API outputs instance
            Out = new ApiMethods(this);
        }

        // Start host
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