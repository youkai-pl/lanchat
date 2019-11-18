using Lanchat.Common.TcpLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lanchat.Common.NetworkLib
{
    public static class Network
    {
        // Users list
        private static List<User> Users = new List<User>();

        public static void Init(int PORT, string nickname, string publicKey)
        {
            string selfHash = Guid.NewGuid().ToString();
            int tcpPort = FreeTcpPort();

            User self = new User(
                nickname,
                publicKey,
                selfHash,
                tcpPort
                );

            // Start host
            Host TcpHost = new Host();
            TcpHost.HostEvent += OnHostEvent;
            Task.Run(() => { TcpHost.Start(tcpPort); });

            // Create UDP client
            var udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            // Start broadcast
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(self));
                    udpClient.Send(data, data.Length, "255.255.255.255", PORT);
                    Thread.Sleep(1000);
                }
            });

            // Listen broadcast
            Task.Run(() =>
            {
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    var sender = from.Address;
                    User paperplane = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(recvBuffer));
                    if (!Users.Exists(x => x.Ip.Equals(sender)) && paperplane.Hash != selfHash)
                    {
                        Users.Add(paperplane);
                        var userIndex = Users.Count - 1;
                        Users[userIndex].Ip = sender;
                        Users[userIndex].Connection = new Client();
                        Users[userIndex].Connection.Connect(sender, paperplane.Port, JsonConvert.SerializeObject(self));
                    }
                }
            });
        }

        public static void SendAll(string message)
        {
            Users.ForEach(x =>
            {
                if (x.Connection != null)
                {
                    x.Connection.Send(message);
                }
            });
        }

        private static void OnHostEvent(object o, EventArgs e)
        {
            // Handle status
            if (o is Host.Status status)
            {
                if (CheckHandshake(status.Ip))
                {
                    // Handle disconnect
                    if (status.Type == "disconnected")
                    {
                        Console.WriteLine("disconnected");
                        Users.RemoveAll(x => x.Ip.Equals(status.Ip));
                    }
                    else if (status.Type == "connected")
                    {
                        Console.WriteLine(status.Ip.ToString());
                        while (!Users.Exists(x => x.Ip.Equals(status.Ip)))
                        {
                            Thread.Sleep(25);
                        }
                        Console.WriteLine("connected");
                        Console.WriteLine(Users.First(x => x.Ip.Equals(status.Ip)).Hash);
                    }
                }
            }

            // Handle message
            if (o is Host.Message message)
            {
                if (CheckHandshake(message.Ip))
                {
                    Console.WriteLine(Users.First(x => x.Ip.Equals(message.Ip)).Nickname + ": " + message.Content);
                }
            }

            //Hadnle handshake
            if (o is Host.Handshake handshake)
            {
                if (!Users.Exists(x => x.Ip.Equals(handshake.Ip)))
                {
                    Users.Add(new User(handshake.Nickname, handshake.PublicKey, handshake.Hash, handshake.Port));
                }
                Users.First(x => x.Ip.Equals(handshake.Ip)).Handshake = true;
            }
        }

        private static bool CheckHandshake(IPAddress ip)
        {
            return Users.First(x => x.Ip.Equals(ip)).Handshake == true;
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