using System;
using System.Collections.ObjectModel;
using Lanchat.Core.Config;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.NodesDiscovery
{
    /// <summary>
    ///     Detecting nodes by UDP broadcasts.
    /// </summary>
    public class NodesDetection
    {
        private readonly AnnounceListener announceListener;
        private readonly AnnounceSender announceSender;

        internal NodesDetection(IConfig config)
        {
            var uniqueId = Guid.NewGuid().ToString();
            var udpClient = new UdpClientWrapper();

            announceListener = new AnnounceListener(config, udpClient, uniqueId, DetectedNodes);
            announceSender = new AnnounceSender(config, udpClient, uniqueId);
        }

        /// <summary>
        ///     Detected nodes.
        /// </summary>
        public ObservableCollection<DetectedNode> DetectedNodes { get; } = new();

        internal void Start()
        {
          //  announceListener.Start();
          //  announceSender.Start();
        }
    }
}