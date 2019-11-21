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
                Trace.WriteLine("Recieved handshake");
            }
        }

        // Check is paperplane come from self or user alredy exist in list
        private static bool IsUserSelfOrAlreadyExist(Paperplane self, Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != self.Id && !NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !NodeList.Exists(x => x.Ip.Equals(senderIp));
        }


        // OLD 

        //// Start host
        //// TcpLib.Host TcpHost = new TcpLib.Host();
        //// TcpHost.HostEvent += OnHostEvent;
        //// Task.Run(() => { TcpHost.Start(tcpPort); });
        //public static void SendAll(string message)
        //{
        //    Users.ForEach(x =>
        //    {
        //        if (x.Connection != null)
        //        {
        //            //x.Connection.Send(message);
        //        }
        //    });
        //}


        //private static void OnHostEvent(object o, EventArgs e)
        //{
        //    // Handle status
        //    if (o is TcpLib.Host.Status status)
        //    {
        //        //if (CheckHandshake(status.Ip))
        //        //{
        //        // Handle disconnect
        //        if (status.Type == "disconnected")
        //        {
        //            Console.WriteLine("disconnected");
        //            Users.RemoveAll(x => x.Ip.Equals(status.Ip));
        //        }
        //        else if (status.Type == "connected")
        //        {
        //            while (!Users.Exists(x => x.Ip.Equals(status.Ip)))
        //            {
        //                Thread.Sleep(25);
        //            }
        //            Console.WriteLine("connected");
        //            Console.WriteLine(Users.First(x => x.Ip.Equals(status.Ip)).Hash);
        //            Console.WriteLine(Users.First(x => x.Ip.Equals(status.Ip)).Nickname);
        //        }
        //        //}
        //    }

        //    // Handle message
        //    if (o is TcpLib.Host.Message message)
        //    {
        //        //if (CheckHandshake(message.Ip))
        //        //{
        //        Console.WriteLine(Users.First(x => x.Ip.Equals(message.Ip)).Nickname + ": " + message.Content);
        //        // }
        //    }

        //    //Hadnle handshake
        //    if (o is TcpLib.Host.Handshake handshake)
        //    {
        //        Console.WriteLine("handhshake");
        //        if (!Users.Exists(x => x.Ip.Equals(handshake.Ip)))
        //        {
        //            Users.Add(new User(handshake.Nickname, handshake.PublicKey, handshake.Hash, handshake.Port));
        //        }
        //        Users.First(x => x.Ip.Equals(handshake.Ip)).Handshake = true;
        //    }
        //}

        //private static bool CheckHandshake(IPAddress ip)
        //{
        //    return Users.Exists(x => x.Ip.Equals(ip)) ? Users.First(x => x.Ip.Equals(ip)).Handshake == true : false;
        //}

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