using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Node
    {
        public readonly NetworkOutput NetworkOutput;
        public readonly NetworkInput NetworkInput;
        internal readonly INetworkElement NetworkElement;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        public Node(INetworkElement networkElement)
        {
            NetworkElement = networkElement;
            NetworkOutput = new NetworkOutput(this);
            NetworkInput = new NetworkInput(this);

            NetworkInput.HandshakeReceived += OnHandshakeReceived;
            networkElement.Connected += OnConnected;
            networkElement.Disconnected += OnDisconnected;
            networkElement.SocketErrored += OnSocketErrored;
            networkElement.DataReceived += NetworkInput.ProcessReceivedData;
        }

        /// <summary>
        ///     Node nickname.
        /// </summary>
        public string Nickname { get; private set; }

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        public bool Ready { get; private set; }

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        public Guid Id => NetworkElement.Id;

        /// <summary>
        ///     IP address of node.
        /// </summary>
        public IPEndPoint Endpoint => NetworkElement.Endpoint;

        /// <summary>
        ///     Node successful connected and ready.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Node disconnected.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        ///     TCP session or client for this node returned error.
        /// </summary>
        public event EventHandler<SocketError> SocketErrored;

        private void OnConnected(object sender, EventArgs e)
        {
            NetworkOutput.SendHandshake();

            // Check is connection established successful after timeout
            Task.Delay(5000).ContinueWith(t =>
            {
                if (!Ready)
                {
                    NetworkElement.Close();
                }
            });
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            SocketErrored?.Invoke(this, e);
        }

        private void OnHandshakeReceived(object sender, Handshake handshake)
        {
            Nickname = handshake.Nickname;
            Ready = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }
    }
}