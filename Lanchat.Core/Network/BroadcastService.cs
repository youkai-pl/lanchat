using System;
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
        private readonly IPEndPoint endPoint;
        private readonly string uniqueId;

        internal EventHandler<IPAddress> BroadcastReceived;

        internal BroadcastService()
        {
            uniqueId = Guid.NewGuid().ToString();
            endPoint = new IPEndPoint(IPAddress.Broadcast, CoreConfig.BroadcastPort);
            udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, CoreConfig.BroadcastPort));
        }

        internal void Start()
        {
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    if (Encoding.UTF8.GetString(recvBuffer) != uniqueId)
                    {
                        BroadcastReceived?.Invoke(this, from.Address);
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var data = Encoding.UTF8.GetBytes(uniqueId);
                    udpClient.Send(data, data.Length, endPoint);
                    Thread.Sleep(2000);
                }
            });
        }
    }
}