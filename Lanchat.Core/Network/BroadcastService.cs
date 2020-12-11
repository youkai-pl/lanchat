using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable FunctionNeverReturns

namespace Lanchat.Core.Network
{
    internal class BroadcastService
    {
        private readonly UdpClient udpClient;
        private readonly int port;

        internal BroadcastService()
        {
            port = 3646;
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, port));
        }

        internal void Start()
        {
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    Trace.WriteLine(Encoding.UTF8.GetString(recvBuffer));
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes("test");
                    udpClient.Send(data, data.Length, "255.255.255.255", port);
                    Thread.Sleep(2000);
                }
            });
        }
    }
}