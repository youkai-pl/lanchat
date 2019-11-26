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
        private static readonly List<Node> NodeList = new List<Node>();

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
            var host = new Host(BroadcastPort);

            // Listen host events
            host.RecievedBroadcast += OnRecievedBroadcast;
            host.NodeConnected += OnNodeConnected;
            host.NodeDisconnected += OnNodeDisconnected;
            host.RecievedHandshake += OnRecievedHandshake;
            host.RecievedMessage += OnRecievedMessage;

            // Initialize host
            host.StartHost(HostPort);

            // Initialize broadcast
            host.Broadcast(new Paperplane(HostPort, Id));

            // Listen other hosts broadcasts
            host.ListenBroadcast();
        }

        // Handle recieved broadcast
        private void OnRecievedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (IsCanAdd(e.Sender, e.SenderIP))
            {
                // Create new node
                CreateNode(e.Sender.Id, e.Sender.Port, e.SenderIP);
            }
        }

        // Handle node connect
        private void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
        }

        // Handle node disconnect
        private void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            try
            {
                // Remove node from list
                Trace.WriteLine(NodeList.Find(x => x.Ip.Equals(e.NodeIP)).Nickname + " disconnected");
                NodeList.RemoveAll(x => x.Ip.Equals(e.NodeIP));
            }
            catch
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        // Handle recieved handshake
        private void OnRecievedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine("Recieved handshake");
            Trace.Indent();
            Trace.WriteLine(e.NodeHandshake.Nickname);
            Trace.WriteLine(e.SenderIP);
            Trace.Unindent();

            if (NodeList.Exists(x => x.Ip.Equals(e.SenderIP)))
            {
                Trace.WriteLine("Node found and handshake accepted");
                NodeList.Find(x => x.Ip.Equals(e.SenderIP)).AcceptHandshake(e.NodeHandshake);
            }
            else
            {
                // Create new node
                Trace.WriteLine("New node created after recieved handshake");
                CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                NodeList.Find(x => x.Ip.Equals(e.SenderIP)).AcceptHandshake(e.NodeHandshake);
            }
        }

        // Handle message
        private void OnRecievedMessage(object o, RecievedMessageEventArgs e)
        {
            var userNickname = NodeList.Find(x => x.Ip.Equals(e.SenderIP)).Nickname;
            Trace.WriteLine(userNickname + ": " + e.Content);
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
        public static void SendAll(string message)
        {
            NodeList.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    x.Connection.SendMessage(message);
                }
            });
        }

        // Check is paperplane come from self or user alredy exist in list
        private bool IsCanAdd(Paperplane broadcast, IPAddress senderIp)
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
    }
}