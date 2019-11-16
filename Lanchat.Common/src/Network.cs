using Lanchat.Common.TcpLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                    if (!Users.Exists(x => x.Hash == paperplane.Hash) && paperplane.Hash != selfHash)
                    {
                        Users.Add(paperplane);
                        var userIndex = Users.Count - 1;
                        Users[userIndex].Ip = sender;
                        Users[userIndex].Connection = new Client();
                        Users[userIndex].Connection.ClientEvent += OnClientEvent;
                        Users[userIndex].Connection.Connect(sender, paperplane.Port);
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

        private static void OnHostEvent(Host.EventObject o, EventArgs e)
        {
            if (o.Type == "connected")
            {
                Console.WriteLine("connected");
            }
            if (o.Type == "disconnected")
            {
                Console.WriteLine("disconnected");
                Users.RemoveAll(u => u.Ip.ToString() == o.Ip.Split(':')[0]);
            }
        }

        private static void OnClientEvent(object o, EventArgs e)
        {
            //Console.WriteLine(o);
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
        }

        public string Nickname { get; set; }
        public string Hash { get; set; }
        public string PublicKey { get; set; }
        public int Port { get; set; }
        public IPAddress Ip { get; set; }
        public Client Connection { get; set; }
    }
}