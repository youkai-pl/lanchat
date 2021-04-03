using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.P2PHandlers;

namespace Lanchat.Core
{
    /// <summary>
    ///     Main class representing network in P2P mode.
    /// </summary>
    public class P2P
    {
        private readonly IConfig config;
        private readonly P2PInternalHandlers networkInternalHandlers;
        internal readonly List<Node> OutgoingConnections = new();
        private readonly Server server;

        /// <summary>
        ///     Initialize P2P mode.
        /// </summary>
        public P2P(IConfig config)
        {
            this.config = config;
            networkInternalHandlers = new P2PInternalHandlers(this, this.config);

            server = this.config.UseIPv6
                ? new Server(IPAddress.IPv6Any, this.config.ServerPort, this.config)
                : new Server(IPAddress.Any, this.config.ServerPort, this.config);

            server.SessionCreated += networkInternalHandlers.OnSessionCreated;
            this.config.PropertyChanged += ConfigOnPropertyChanged;
            NodesDetection = new NodesDetection(this.config);
            Broadcasting = new Broadcasting(this);
        }

        /// <see cref="NodesDetection" />
        public NodesDetection NodesDetection { get; }

        /// <summary>
        ///     List of connected nodes.
        /// </summary>
        public List<Node> Nodes
        {
            get
            {
                var nodes = new List<Node>();
                nodes.AddRange(OutgoingConnections);
                nodes.AddRange(server.IncomingConnections);
                return nodes.Where(x => x.Ready).ToList();
            }
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
            if (config.StartServer) server.Start();
            if (config.NodesDetection) NodesDetection.Start();
            if (config.ConnectToSaved) config.SavedAddresses.ForEach(async x => await Connect(x));
        }

        /// <summary>
        ///     Connect to node.
        /// </summary>
        /// <param name="ipAddress">Node IP address.</param>
        /// <param name="port">Node port.</param>
        public Task<bool> Connect(IPAddress ipAddress, int? port = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            port ??= config.ServerPort;
            CheckAddress(ipAddress);
            var client = new Client(ipAddress, port.Value);
            var node = new Node(client, config, false);
            node.Resolver.RegisterHandler(new NodesListHandler(this, config));
            OutgoingConnections.Add(node);
            SubscribeEvents(node, tcs);
            OnNodeCreated(node);
            client.ConnectAsync();
            return tcs.Task;
        }

        private void CheckAddress(IPAddress ipAddress)
        {
            if (config.BlockedAddresses.Contains(ipAddress)) throw new ArgumentException("Node blocked");
            if (Nodes.Any(x => x.NetworkElement.Endpoint.Address.Equals(ipAddress)))
                throw new ArgumentException("Already connected to this node");
        }

        private void SubscribeEvents(Node node, TaskCompletionSource<bool> tcs)
        {
            node.Connected += (_, _) => { tcs.TrySetResult(true); };
            node.CannotConnect += (_, _) => { tcs.TrySetResult(false); };
            node.Connected += networkInternalHandlers.OnConnected;
            node.Disconnected += networkInternalHandlers.CloseNode;
            node.CannotConnect += networkInternalHandlers.CloseNode;
        }
        
        private void ConfigOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Nickname":
                    Broadcasting.SendData(new NicknameUpdate {NewNickname = config.Nickname});
                    break;

                case "Status":
                    Broadcasting.SendData(new StatusUpdate {NewStatus = config.Status});
                    break;
            }
        }
        
        internal void OnNodeCreated(Node e)
        {
            NodeCreated?.Invoke(this, e);
        }
    }
}