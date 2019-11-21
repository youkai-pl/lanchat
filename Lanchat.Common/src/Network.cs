using Lanchat.Common.HostLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

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
            host.RecievedBroadcast += OnRecievedBroadcast;
            host.RecievedHandshake += OnRecievedHandshake;

            // Initialize host
            host.StartHost(tcpPort);

            // Initialize broadcast
            host.Broadcast(self);

            // Listen other hosts broadcasts
            host.ListenBroadcast();

            // Recieved broadcast hadnle
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

            // Handle recieved handshake
            void OnRecievedHandshake(params object[] arguments)
            {
                var handshake = (Handshake)arguments[0];
                var ip = (IPAddress)arguments[1];

                Trace.WriteLine("Recieved handshake");
                Trace.WriteLine(handshake.Nickname);
                Trace.WriteLine(ip);

                try
                {
                    NodeList.Find(x => x.Ip.Equals(ip)).AcceptHandshake(handshake);
                }
                catch
                {
                    // Create new node
                    CreateNode(handshake.Id, handshake.Port, ip);
                }
            }

            // Create new node
            void CreateNode(Guid id, int port, IPAddress ip, Handshake handshake = null)
            {
                var node = new Node(id, port, ip);
                node.CreateConnection(new Handshake(nickname, publicKey, selfId, tcpPort));
                NodeList.Add(node);

                Trace.WriteLine("New node created");
                Trace.WriteLine(node.Id.ToString());
                Trace.WriteLine(node.Port.ToString());

                // Auto handshake
                if (handshake != null)
                {
                    node.AcceptHandshake(handshake);
                    Trace.WriteLine("Auto handshake");
                }
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