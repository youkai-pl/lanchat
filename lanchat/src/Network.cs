using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lanchat.Network
{
    public static class Client
    {
        public static void Init(int PORT, string nickname, string publicKey)
        {
            string selfHash = Guid.NewGuid().ToString();

            XElement self = new XElement("paperplane",
                new XElement("nickname", nickname),
                new XElement("publickey", publicKey),
                new XElement("hash", selfHash));

            // create UDP client
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            // broadcast
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(self.ToString());
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
                    var paperplane = XElement.Parse(Encoding.UTF8.GetString(recvBuffer));

                    if (paperplane.Element("hash").Value != selfHash)
                    {
                        Console.WriteLine("");
                        Console.WriteLine(sender);
                        Console.WriteLine(paperplane.Element("nickname").Value);
                        Console.WriteLine(paperplane.Element("hash").Value);
                    }
                }
            });
        }
    }
}