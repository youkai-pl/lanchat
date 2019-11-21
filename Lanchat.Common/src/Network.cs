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
                    NodeList.Add(new Node(broadcast.Id, broadcast.Port, senderIp));
                    // Crate connection
                    NodeList[NodeList.Count - 1].CreateConnection(new Handshake(nickname, publicKey));

                    Trace.WriteLine("New node created");
                    Trace.WriteLine(broadcast.Id.ToString());
                    Trace.WriteLine(broadcast.Port.ToString());
                    Trace.WriteLine(senderIp.ToString());
                }
            }

            void OnRecievedHandshake(params object[] arguments)
            {
                var handshake = (Handshake)arguments[0];
                var ip = (IPAddress)arguments[1];
                Trace.WriteLine("Recieved handshake");
                Trace.WriteLine(handshake.Nickname);
                Trace.WriteLine(ip);
            }
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