using Lanchat.Common.HostLib;
using Lanchat.Common.ClientLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Lanchat.Common.NetworkLib
{
    public static class Network
    {
        // Users list
        private static List<User> Users = new List<User>();

        public static void Init(int port, string nickname, string publicKey)
        {

            // Generate id
            var selfId = Guid.NewGuid();

            // Find free port
            var tcpPort = FreeTcpPort();

            // Self
            var self = new
            {
                id = selfId,
                port = tcpPort
            };

            // Create client class
            Client client = new Client();
            client.RecievedBroadcast += OnRecievedBroadcast;

            // Create UDP client
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));

            // Initialize broadcast
            Host.Broadcast(udpClient, self, port);

            // Listen broadcast
            client.ListenBroadcast(udpClient);

            void OnRecievedBroadcast(params object[] arguments)
            {
                var paperplane = (Paperplane)arguments[0];
                if (paperplane.Id != self.id)
                {
                    Trace.WriteLine("[OK]");
                }
                else
                {
                    Trace.WriteLine("Self paperplane ignored");
                }
            }
        }


        // OLD 

        // Start host
        // TcpLib.Host TcpHost = new TcpLib.Host();
        // TcpHost.HostEvent += OnHostEvent;
        // Task.Run(() => { TcpHost.Start(tcpPort); });
        public static void SendAll(string message)
        {
            Users.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    //x.Connection.Send(message);
                }
            });
        }

        private static void OnHostEvent(object o, EventArgs e)
        {
            // Handle status
            if (o is TcpLib.Host.Status status)
            {
                //if (CheckHandshake(status.Ip))
                //{
                // Handle disconnect
                if (status.Type == "disconnected")
                {
                    Console.WriteLine("disconnected");
                    Users.RemoveAll(x => x.Ip.Equals(status.Ip));
                }
                else if (status.Type == "connected")
                {
                    while (!Users.Exists(x => x.Ip.Equals(status.Ip)))
                    {
                        Thread.Sleep(25);
                    }
                    Console.WriteLine("connected");
                    Console.WriteLine(Users.First(x => x.Ip.Equals(status.Ip)).Hash);
                    Console.WriteLine(Users.First(x => x.Ip.Equals(status.Ip)).Nickname);
                }
                //}
            }

            // Handle message
            if (o is TcpLib.Host.Message message)
            {
                //if (CheckHandshake(message.Ip))
                //{
                Console.WriteLine(Users.First(x => x.Ip.Equals(message.Ip)).Nickname + ": " + message.Content);
                // }
            }

            //Hadnle handshake
            if (o is TcpLib.Host.Handshake handshake)
            {
                Console.WriteLine("handhshake");
                if (!Users.Exists(x => x.Ip.Equals(handshake.Ip)))
                {
                    Users.Add(new User(handshake.Nickname, handshake.PublicKey, handshake.Hash, handshake.Port));
                }
                Users.First(x => x.Ip.Equals(handshake.Ip)).Handshake = true;
            }
        }

        private static bool CheckHandshake(IPAddress ip)
        {
            return Users.Exists(x => x.Ip.Equals(ip)) ? Users.First(x => x.Ip.Equals(ip)).Handshake == true : false;
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

    // User class
    public class User
    {
        public User(string nickname, string publicKey, string hash, int port)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Hash = hash;
            Port = port;
            Handshake = false;
        }

        public string Nickname { get; set; }
        public string Hash { get; set; }
        public string PublicKey { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Connection { get; set; }
        public Boolean Handshake { get; set; }
    }
}