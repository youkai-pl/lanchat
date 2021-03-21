using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Timer = System.Timers.Timer;

// ReSharper disable FunctionNeverReturns

namespace Lanchat.Core.Network
{
    public class Broadcasting
    {
        private readonly IConfig config;
        private readonly IPEndPoint endPoint;
        private readonly UdpClient udpClient;
        private readonly string uniqueId;

        internal Broadcasting(IConfig config)
        {
            this.config = config;
            uniqueId = Guid.NewGuid().ToString();
            endPoint = new IPEndPoint(IPAddress.Broadcast, config.BroadcastPort);
            udpClient = new UdpClient();
        }

        public ObservableCollection<Broadcast> DetectedNodes { get; } = new();

        internal void Start()
        {
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, config.BroadcastPort));
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    try
                    {
                        var broadcast = JsonSerializer.Deserialize<Broadcast>(Encoding.UTF8.GetString(recvBuffer));
                        if (broadcast != null && broadcast.Guid != uniqueId)
                        {
                            broadcast.IpAddress = from.Address;
                            broadcast.Nickname = broadcast.Nickname.Truncate(config.MaxNicknameLenght);
                            BroadcastReceived(broadcast);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is JsonException ||
                            e is ArgumentException ||
                            e is NotSupportedException)
                            return;

                        throw;
                    }
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    var json = JsonSerializer.Serialize(new Broadcast
                    {
                        Guid = uniqueId,
                        Nickname = config.Nickname
                    });

                    var data = Encoding.UTF8.GetBytes(json);
                    udpClient.Send(data, data.Length, endPoint);
                    Thread.Sleep(2000);
                }
            });
        }

        // UDP broadcast received
        private void BroadcastReceived(Broadcast e)
        {
            var alreadyDetected = DetectedNodes.FirstOrDefault(x => Equals(x.IpAddress, e.IpAddress));
            if (alreadyDetected == null)
            {
                DetectedNodes.Add(e);
                e.Active = true;

                var timer = new Timer
                {
                    Interval = 2500,
                    Enabled = true
                };

                timer.Elapsed += (_, _) =>
                {
                    if (e.Active)
                    {
                        e.Active = false;
                    }
                    else
                    {
                        timer.Dispose();
                        DetectedNodes.Remove(e);
                    }
                };
            }
            else
            {
                alreadyDetected.Active = true;
                alreadyDetected.Nickname = e.Nickname;
            }
        }
    }
}