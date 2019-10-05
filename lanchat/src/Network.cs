using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lanchat.Network
{
    public static class Client
    {
        public static void Init(int PORT)
        {
            // create UDP client
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            // broadcast
            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes("test");
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
                    //Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                }
            });
        }
    }
}