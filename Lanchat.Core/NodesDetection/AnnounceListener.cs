using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodesDetection
{
    // TODO: Refactor
    internal class AnnounceListener
    {
        private readonly UdpClient udpClient;
        private readonly IConfig config;
        private readonly string uniqueId;
        private readonly ObservableCollection<Announce> detectedNodes;
        private readonly JsonReader jsonReader;

        public AnnounceListener(
            IConfig config,
            UdpClient udpClient,
            string uniqueId,
            ObservableCollection<Announce> detectedNodes)
        {
            this.udpClient = udpClient;
            this.config = config;
            this.uniqueId = uniqueId;
            this.detectedNodes = detectedNodes;
            jsonReader = new JsonReader();
        }

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
                        var broadcast = jsonReader.Deserialize<Announce>(Encoding.UTF8.GetString(recvBuffer));
                        Validator.ValidateObject(broadcast!, new ValidationContext(broadcast), true);
                        
                        if (broadcast.Guid != uniqueId)
                        {
                            broadcast.IpAddress = from.Address;
                            BroadcastReceived(broadcast);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is not JsonException &&
                            e is not ArgumentException &&
                            e is not NotSupportedException &&
                            e is not ValidationException) throw;
                    }
                }
            });
        }

        private void BroadcastReceived(Announce e)
        {
            var alreadyDetected = detectedNodes.FirstOrDefault(x => Equals(x.IpAddress, e.IpAddress));
            if (alreadyDetected == null)
            {
                detectedNodes.Add(e);
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
                        detectedNodes.Remove(e);
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