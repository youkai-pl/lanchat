using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.Extensions;
using Lanchat.Core.Network;
using Lanchat.Core.NodesDetection;
using Lanchat.Core.P2PHandlers;

namespace Lanchat.Core
{
    /// <summary>
    ///     Main class representing network in P2P mode.
    /// </summary>
    public class P2P
    {
        internal readonly IConfig Config;
        internal readonly Server Server;
        private readonly NodesControl nodesControl;

        /// <summary>
        ///     Initialize P2P mode.
        /// </summary>
        public P2P(IConfig config)
        {
            Config = config;
            nodesControl = new NodesControl(config);

            Server = Config.UseIPv6
                ? new Server(IPAddress.IPv6Any, Config.ServerPort, Config, nodesControl)
                : new Server(IPAddress.Any, Config.ServerPort, Config, nodesControl);

            Server.SessionCreated += (sender, node) => { NodeCreated?.Invoke(sender, node); };
            NodesDetection = new NodesDetector(Config);
            Broadcasting = new Broadcasting(this);
            _ = new ConfigObserver(this);
        }

        /// <see cref="NodesDetection" />
        public NodesDetector NodesDetection { get; }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get { return nodesControl.Nodes.Where(x => x.Ready).ToList(); }
        }

        /// <summary>
        ///     Send data to all nodes.
        /// </summary>
        public Broadcasting Broadcasting { get; }

        /// <summary>
        ///     New node connected. After receiving this handlers for node events can be created.
        /// </summary>
        public event EventHandler<Node> NodeCreated;

        /// <summary>
        ///     Start server.
        /// </summary>
        public void Start()
        {
            if (Config.StartServer) Server.Start();
            if (Config.NodesDetection) NodesDetection.Start();
            if (Config.ConnectToSaved) Config.SavedAddresses.ForEach(x => Connect(x));
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
            var node = new Node(client, Config, false);
            nodesControl.AddNode(node);
            node.Resolver.RegisterHandler(new NodesListHandler(this));
            SubscribeEvents(node, tcs);
            NodeCreated?.Invoke(this, node);
            client.ConnectAsync();
            return tcs.Task;
        }

        [SuppressMessage("ReSharper", "ParameterOnlyUsedForPreconditionCheck.Local")]
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