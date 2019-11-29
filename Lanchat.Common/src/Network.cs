using Lanchat.Common.HostLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    public class Network
    {
        // Users list
        public List<Node> NodeList = new List<Node>();

        // Host
        private readonly Host host;

        // Properties
        public string Nickname { get; set; }
        public string PublicKey { get; set; }
        public int BroadcastPort { get; set; }
        public int HostPort { get; set; }
        public Guid Id { get; set; }

        public Network(int port, string nickname, string publicKey)
        {
            // Set properties
            Nickname = nickname;
            PublicKey = publicKey;
            BroadcastPort = port;
            Id = Guid.NewGuid();
            HostPort = FreeTcpPort();

            // Create host class
            host = new Host(BroadcastPort);

            // Listen host events
            var handlers = new NetworkHandlers(this);
            host.RecievedBroadcast += handlers.OnRecievedBroadcast;
            host.NodeConnected += handlers.OnNodeConnected;
            host.NodeDisconnected += handlers.OnNodeDisconnected;
            host.RecievedHandshake += handlers.OnRecievedHandshake;
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

        // Create new node
        public void CreateNode(Guid id, int port, IPAddress ip)
        {
            var node = new Node(id, port, ip);
            node.CreateConnection(new Handshake(Nickname, PublicKey, Id, HostPort));
            NodeList.Add(node);
            Trace.WriteLine("New node created");
            Trace.Indent();
            Trace.WriteLine(node.Id.ToString());
            Trace.WriteLine(node.Port.ToString());
            Trace.WriteLine(node.Nickname);
            Trace.Unindent();
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