using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Lanchat.Core.Config;
using Lanchat.Core.Json;
using Lanchat.Core.NodesDiscovery.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.NodesDiscovery
{
    internal class AnnounceListener
    {
        private readonly IConfig config;
        private readonly ObservableCollection<DetectedNode> detectedNodes;
        private readonly JsonUtils jsonUtils;
        private readonly IUdpClient udpClient;
        private readonly string uniqueId;

        public AnnounceListener(
            IConfig config,
            IUdpClient udpClient,
            string uniqueId,
            ObservableCollection<DetectedNode> detectedNodes)
        {
            this.udpClient = udpClient;
            this.config = config;
            this.uniqueId = uniqueId;
            this.detectedNodes = detectedNodes;
            jsonUtils = new JsonUtils();
        }

        internal void Start()
        {
            udpClient.Bind(new IPEndPoint(IPAddress.Any, config.BroadcastPort));
            var from = new IPEndPoint(0, 0);
            Task.Run(() =>
            {
                while (true)
                {
                    var buffer = udpClient.Receive(ref from);
                    try
                    {
                        HandleBroadcast(buffer, from);
                    }
                    catch (JsonException)
                    { }
                    catch (InvalidOperationException)
                    { }
                }
            });
        }

        private void HandleBroadcast(string json, IPEndPoint from)
        {
            var broadcast = jsonUtils.Deserialize<Announce>(json);
            if (!CheckPreconditions(broadcast))
            {
                return;
            }

            BroadcastReceived(broadcast, from.Address);
        }

        private bool CheckPreconditions(Announce broadcast)
        {
            if (Validator.TryValidateObject(broadcast,
                new ValidationContext(broadcast),
                new List<ValidationResult>()))
            {
                return broadcast.Guid != uniqueId;
            }

            return false;
        }

        private void BroadcastReceived(Announce announce, IPAddress ipAddress)
        {
            var alreadyDetected = detectedNodes.FirstOrDefault(x => Equals(x.IpAddress, ipAddress));
            if (alreadyDetected == null)
            {
                AddNewNode(announce, ipAddress);
            }
            else
            {
                alreadyDetected.Active = true;
                alreadyDetected.Nickname = announce.Nickname;
            }
        }

        private void AddNewNode(Announce announce, IPAddress ipAddress)
        {
            var detectedNode = new DetectedNode
            {
                Nickname = announce.Nickname,
                IpAddress = ipAddress,
                Active = true
            };

            detectedNodes.Add(detectedNode);
            SetupTimer(detectedNode);
        }

        private void SetupTimer(DetectedNode detectedNode)
        {
            var timer = new Timer
            {
                Interval = 5000,
                Enabled = true
            };

            timer.Elapsed += (_, _) =>
            {
                if (detectedNode.Active)
                {
                    detectedNode.Active = false;
                    return;
                }

                timer.Dispose();
                detectedNodes.Remove(detectedNode);
            };
        }
    }
}