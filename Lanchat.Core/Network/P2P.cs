using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Extensions;
using Lanchat.Core.NodesDetection;
using Lanchat.Core.TransportLayer;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable IntroduceOptionalParameters.Global

namespace Lanchat.Core.Network
{
    /// <inheritdoc />
    public class P2P : IP2P
    {
        internal readonly IConfig Config;
        private readonly NodesControl nodesControl;
        private readonly Server server;

        /// <summary>
        ///     Initialize P2P mode
        /// </summary>
        /// <param name="config">Lanchat config</param>
        public P2P(IConfig config) : this(config, null)
        { }

        /// <summary>
        ///     Initialize P2P mode
        /// </summary>
        /// <param name="config">Lanchat config</param>
        /// <param name="apiHandlers">Custom api handlers</param>
        public P2P(IConfig config, IEnumerable<Type> apiHandlers)
        {
            Config = config;
            var container = NodeSetup.Setup(config, this, apiHandlers);
            nodesControl = new NodesControl(config, container);

            nodesControl.NodeCreated += (sender, node) =>
            {
                var inode = (INode) node;
                NodeCreated?.Invoke(sender, inode);
            };

            server = Config.UseIPv6
                ? new Server(IPAddress.IPv6Any, Config.ServerPort, Config, nodesControl)
                : new Server(IPAddress.Any, Config.ServerPort, Config, nodesControl);

            NodesDetection = new NodesDetector(Config);
            Broadcast = new Broadcast(nodesControl.Nodes);
            _ = new ConfigObserver(this);
        }

        /// <inheritdoc />
        public NodesDetector NodesDetection { get; }

        /// <inheritdoc />
        public List<INode> Nodes => nodesControl.Nodes.Where(x => x.Ready).Cast<INode>().ToList();

        /// <inheritdoc />
        public IBroadcast Broadcast { get; }

        /// <inheritdoc />
        public event EventHandler<INode> NodeCreated;

        /// <inheritdoc />
        public void Start()
        {
            if (Config.StartServer)
            {
                server.Start();
            }

            if (Config.NodesDetection)
            {
                NodesDetection.Start();
            }

            if (Config.ConnectToSaved)
            {
                ConnectToSavedAddresses();
            }
        }

        /// <inheritdoc />
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

        private void ConnectToSavedAddresses()
        {
            Config.SavedAddresses.ForEach(x =>
            {
                try
                {
                    Connect(x);
                }
                catch (ArgumentException)
                { }
            });
        }

        private void CheckAddress(IPAddress ipAddress)
        {
            if (Config.BlockedAddresses.Contains(ipAddress))
            {
                throw new ArgumentException("Node blocked");
            }

            if (Nodes.Any(x => x.Host.Endpoint.Address.Equals(ipAddress)))
            {
                throw new ArgumentException("Already connected to this node");
            }

            var localHost = Dns.GetHostEntry(Dns.GetHostName());
            if ((localHost.AddressList.Any(x => x.Equals(ipAddress)) ||
                ipAddress.Equals(IPAddress.Loopback) ||
                ipAddress.Equals(IPAddress.IPv6Loopback)) 
                && !Config.DebugMode)
            {
                throw new ArgumentException("Address belong to local machine");
            }
        }

        private static void SubscribeEvents(INodeInternal node, TaskCompletionSource<bool> tcs)
        {
            node.Connected += (_, _) => { tcs.TrySetResult(true); };
            node.CannotConnect += (_, _) => { tcs.TrySetResult(false); };
        }
    }
}