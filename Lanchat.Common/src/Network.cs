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

        // Send message to all nodes
        public void SendAll(string message)
        {
            NodeList.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    x.Connection.SendMessage(message);
                }
            });
        }

        // Change nickname
        public void ChangeNickname(string nickname)
        {
            NodeList.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    x.Connection.SendNickname(nickname);
                }
            });
        }

        // Check is paperplane come from self or user alredy exist in list
        public bool IsCanAdd(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != Id && !NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !NodeList.Exists(x => x.Ip.Equals(senderIp));
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

        // Recieved message event
        public event EventHandler<RecievedMessageEventArgs> RecievedMessage;

        public virtual void OnRecievedMessage(string content, string nickname)
        {
            RecievedMessage(this, new RecievedMessageEventArgs()
            {
                Content = content,
                Nickname = nickname
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEvent> NodeConnected;

        public virtual void OnNodeConnected(IPAddress ip, string nickname)
        {
            NodeConnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        // Node disconnected event
        public event EventHandler<NodeConnectionStatusEvent> NodeDisconnected;

        public virtual void OnNodeDisconnected(IPAddress ip, string nickname)
        {
            NodeDisconnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        // Changed nickname event
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        public virtual void OnChangedNickname(string oldNickname, string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                OldNickname = oldNickname,
                SenderIP = senderIP
            });
        }
    }
}