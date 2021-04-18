using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.API;
using Lanchat.Core.Extensions;
using Lanchat.Core.Network;
using Lanchat.Core.NodesDetection;

namespace Lanchat.Core
{
    /// <summary>
    ///     Main class representing network in P2P mode.
    /// </summary>
    public class P2P
    {
        internal readonly IConfig Config;
        private readonly NodesControl nodesControl;
        private readonly Server server;

        /// <summary>
        ///     Initialize P2P mode.
        /// </summary>
        public P2P(IConfig config)
        {
            Config = config;
            nodesControl = new NodesControl(config, this);
            nodesControl.NodeCreated += (sender, node) => { NodeCreated?.Invoke(sender, node); };

            server = Config.UseIPv6
                ? new Server(IPAddress.IPv6Any, Config.ServerPort, Config, nodesControl)
                : new Server(IPAddress.Any, Config.ServerPort, Config, nodesControl);

            NodesDetection = new NodesDetector(Config);
            Broadcast = new Broadcast(nodesControl.Nodes);
            _ = new ConfigObserver(this);
        }

        /// <see cref="NodesDetection" />
        public NodesDetector NodesDetection { get; }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes => nodesControl.Nodes.Where(x => x.Ready).ToList();

        /// <summary>
        ///     Send data to all nodes.
        /// </summary>
        public Broadcast Broadcast { get; }

        /// <summary>
        ///     New node connected. After receiving this handlers for node events can be created.
        /// </summary>
        public event EventHandler<Node> NodeCreated;

        /// <summary>
        ///     Start server.
        /// </summary>
        public void Start()
        {
            if (Config.StartServer) server.Start();
            if (Config.NodesDetection) NodesDetection.Start();
            if (Config.ConnectToSaved)
                Config.SavedAddresses.ForEach(x =>
                {
                    try
                    {
                        Connect(x);
                    }
                    catch (ArgumentException)
                    {
                    }
                });
        }

        /// <summary>
        ///     Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        /// <param name="port">Node port.</param>
        public Task<bool> Connect(IPAddress ipAddress, int? port = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            port ??= Config.ServerPort;
            CheckAddress(ipAddress);
            var client = new Client(ipAddress, port.Value);
            var node = nodesControl.CreateNode(client);
            SubscribeEvents(node, tcs);
            client.ConnectAsync();
            return tcs.Task;
        }

        private void CheckAddress(IPAddress ipAddress)
        {
            if (Config.BlockedAddresses.Contains(ipAddress)) throw new ArgumentException("Node blocked");
            if (Nodes.Any(x => x.NetworkElement.Endpoint.Address.Equals(ipAddress)))
                throw new ArgumentException("Already connected to this node");
        }

        private static void SubscribeEvents(Node node, TaskCompletionSource<bool> tcs)
        {
            node.Connected += (_, _) => { tcs.TrySetResult(true); };
            node.CannotConnect += (_, _) => { tcs.TrySetResult(false); };
        }
    }
}