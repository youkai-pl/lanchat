using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace lanchat.NetworkLib
{
    public static class Client
    {
        public static void Init(int PORT, string nickname, string publicKey)
        {
            string selfHash = Guid.NewGuid().ToString();

            Paperplane self = new Paperplane(
                nickname,
                publicKey,
                selfHash
            );

            // create UDP client
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            // broadcast
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(self));
                    udpClient.Send(data, data.Length, "255.255.255.255", PORT);
                    Thread.Sleep(1000);
                }
            });

            // listen
            Task.Run(() =>
            {
                var from = new IPEndPoint(0, 0);
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    var sender = from.Address.ToString();

                    Paperplane paperplane = JsonConvert.DeserializeObject<Paperplane>(Encoding.UTF8.GetString(recvBuffer));

                    if (paperplane.Hash != selfHash)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(sender);
                        Console.WriteLine(paperplane.Nickname);
                        Console.WriteLine(paperplane.Hash);
                    }
                }
            });
        }
    }

    public class Paperplane
    {
        public Paperplane(string nickname, string publicKey, string hash)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Hash = hash;
        }

        public string Nickname { get; set; }
        public string Hash { get; set; }
        public string PublicKey { get; set; }

    }
}