using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac.Core;
using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.NodesDetection;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    /// <inheritdoc />
    public class P2P : IP2P
    {
        internal readonly IConfig Config;
        private readonly AddressChecker addressChecker;
        private readonly NodesControl nodesControl;
        private readonly Server server;

        /// <summary>
        ///     Initialize P2P mode
        /// </summary>
        /// <param name="config">Lanchat config</param>
        /// <param name="rsaDatabase">IRsaDatabase implementation</param>
        /// <param name="nodeCreated">Method called after creation of new node</param>
        /// <param name="apiHandlers">Optional custom api handlers</param>
        public P2P(
            IConfig config,
            IRsaDatabase rsaDatabase,
            Action<IActivatedEventArgs<INode>> nodeCreated,
            IEnumerable<Type> apiHandlers = null)
        {
            Config = config;
            LocalRsa = new LocalRsa(rsaDatabase);
            var container = NodeSetup.Setup(config, rsaDatabase, LocalRsa, this, nodeCreated, apiHandlers);
            addressChecker = new AddressChecker(config);
            nodesControl = new NodesControl(config, container, addressChecker);
            server = Config.UseIPv6
                ? new Server(IPAddress.IPv6Any, Config.ServerPort, nodesControl, addressChecker)
                : new Server(IPAddress.Any, Config.ServerPort, nodesControl, addressChecker);

            NodesDetection = new NodesDetector(Config);
            Broadcast = new Broadcast(nodesControl.Nodes);
            _ = new ConfigObserver(this);

            NodesDetection.DetectedNodes.CollectionChanged += ConnectToDetectedAddresses;
        }

        /// <inheritdoc />
        public NodesDetector NodesDetection { get; }

        /// <inheritdoc />
        public List<INode> Nodes => nodesControl.Nodes.Where(x => x.Ready).Cast<INode>().ToList();

        /// <inheritdoc />
        public IBroadcast Broadcast { get; }

        /// <inheritdoc />
        public ILocalRsa LocalRsa { get; }

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
            addressChecker.CheckAddress(ipAddress);
            addressChecker.LockAddress(ipAddress);
            var tcs = new TaskCompletionSource<bool>();
            port ??= Config.ServerPort;
            var client = new Client(ipAddress, port.Value);
            var node = nodesControl.CreateNode(client);
            SubscribeEvents(node, tcs);
            client.ConnectAsync();
            return tcs.Task;
        }

        private void ConnectToDetectedAddresses(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems == null)
            {
                return;
            }

            foreach (DetectedNode newNode in args.NewItems)
            {
                try
                {
                    Connect(newNode.IpAddress);
                }
                catch (ArgumentException)
                { }
            }
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

        private static void SubscribeEvents(INodeInternal node, TaskCompletionSource<bool> tcs)
        {
            node.Connected += (_, _) => { tcs.TrySetResult(true); };
            node.CannotConnect += (_, _) => { tcs.TrySetResult(false); };
        }
    }
}