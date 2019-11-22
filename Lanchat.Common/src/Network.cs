using Lanchat.Common.HostLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    public static class Network
    {
        // Users list
        private static List<Node> NodeList = new List<Node>();

        public static void Init(int udpPort, string nickname, string publicKey)
        {
            // Generate id
            var selfId = Guid.NewGuid();

            // Find free port
            var tcpPort = FreeTcpPort();

            // Self
            var self = new Paperplane(tcpPort, selfId);

            // Create host class
            var host = new Host(udpPort);

            // Listen host events
            host.RecievedBroadcast += OnRecievedBroadcast;
            host.NodeConnected += OnNodeConnected;
            host.NodeDisconnected += OnNodeDisconnected;
            host.RecievedHandshake += OnRecievedHandshake;
            host.RecievedMessage += OnRecievedMessage;

            // Initialize host
            host.StartHost(tcpPort);

            // Initialize broadcast
            host.Broadcast(self);

            // Listen other hosts broadcasts
            host.ListenBroadcast();

            // Handle recieved broadcast
            void OnRecievedBroadcast(params object[] arguments)
            {
                var broadcast = (Paperplane)arguments[0];
                var senderIp = (IPAddress)arguments[1];

                if (IsUserSelfOrAlreadyExist(self, broadcast, senderIp))
                {
                    // Create new node
                    CreateNode(broadcast.Id, broadcast.Port, senderIp);
                }
            }

            // Handle node connect
            void OnNodeConnected(params object[] arguments)
            {
                Trace.WriteLine("New connection from: " + arguments[0].ToString());
            }

            // Handle node disconnect
            void OnNodeDisconnected(params object[] arguments)
            {
                var ip = (IPAddress)arguments[0];

                try
                {
                    // Remove node from list
                    Trace.WriteLine(NodeList.Find(x => x.Ip.Equals(ip)).Nickname + " disconnected");
                    NodeList.RemoveAll(x => x.Ip.Equals(ip));
                }
                catch
                {
                    Trace.WriteLine("Node does not exist");
                }
            }

            // Handle recieved handshake
            void OnRecievedHandshake(params object[] arguments)
            {
                var handshake = (Handshake)arguments[0];
                var ip = (IPAddress)arguments[1];

                Trace.WriteLine("Recieved handshake");
                Trace.Indent();
                Trace.WriteLine(handshake.Nickname);
                Trace.WriteLine(ip);
                Trace.Unindent();

                if (NodeList.Exists(x => x.Ip.Equals(ip)))
                {
                    Trace.WriteLine("Node found and handshake accepted");
                    NodeList.Find(x => x.Ip.Equals(ip)).AcceptHandshake(handshake);
                }
                else
                {
                    // Create new node
                    Trace.WriteLine("New node created after recieved handshake");
                    CreateNode(handshake.Id, handshake.Port, ip);
                    NodeList.Find(x => x.Ip.Equals(ip)).AcceptHandshake(handshake);
                }
            }

            // Handle message
            void OnRecievedMessage(params object[] arguments)
            {
                var message = (string)arguments[0];
                var ip = (IPAddress)arguments[1];

                var userNickname = NodeList.Find(x => x.Ip.Equals(ip)).Nickname;
                Trace.WriteLine(userNickname + ": " + message);
            }

            // Create new node
            void CreateNode(Guid id, int port, IPAddress ip)
            {
                var node = new Node(id, port, ip);
                node.CreateConnection(new Handshake(nickname, publicKey, selfId, tcpPort));
                NodeList.Add(node);

                Trace.WriteLine("New node created");
                Trace.Indent();
                Trace.WriteLine(node.Id.ToString());
                Trace.WriteLine(node.Port.ToString());
                Trace.WriteLine(node.Nickname);
                Trace.Unindent();
            }
        }

        // Send message to all nodes
        public static void SendAll(string message)
        {
            NodeList.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    x.Connection.Send(message);
                }
            });
        }

        // Check is paperplane come from self or user alredy exist in list
        private static bool IsUserSelfOrAlreadyExist(Paperplane self, Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != self.Id && !NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !NodeList.Exists(x => x.Ip.Equals(senderIp));
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