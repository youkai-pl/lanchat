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

        // Tcp client
        private static List<Client> Connections = new List<Client>();

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
            TcpHost.TcpEvent += OnTcpEvent;
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
                    var sender = from.Address.ToString();

                    User paperplane = JsonConvert.DeserializeObject<User>(Encoding.UTF8.GetString(recvBuffer));
                    if (!Users.Exists(x => x.Hash == paperplane.Hash) && paperplane.Hash != selfHash)
                    {
                        Users.Add(paperplane);
                        // LogDetected(sender, paperplane);

                        Connections.Add(new Client());
                        Connections[Connections.Count - 1].TcpEvent += OnTcpEvent;
                        Connections[Connections.Count - 1].Connect(sender, paperplane.Port);
                    }
                }
            });
        }

        public static void SendAll(string message)
        {
            Connections.ForEach(x => { x.Send(message); });
        }

        private static void OnTcpEvent(object o, EventArgs e)
        {
            Console.WriteLine(o);
        }

        // Log detected users info
        private static void LogDetected(string sender, User paperplane)
        {
            Console.WriteLine(sender);
            Console.WriteLine(paperplane.Nickname);
            Console.WriteLine(paperplane.Hash);
            Console.WriteLine(paperplane.Port);
            Console.WriteLine("");
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
    }
}