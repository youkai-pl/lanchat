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
        internal readonly Encryption Encryption;
        internal readonly INetworkElement NetworkElement;
        public readonly NetworkInput NetworkInput;
        public readonly NetworkOutput NetworkOutput;
        private string nickname;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        public Node(INetworkElement networkElement)
        {
            NetworkElement = networkElement;
            NetworkOutput = new NetworkOutput(this);
            NetworkInput = new NetworkInput(this);
            Encryption = new Encryption();

            NetworkInput.HandshakeReceived += OnHandshakeReceived;
            NetworkInput.KeyInfoReceived += OnKeyInfoReceived;

            networkElement.Connected += OnConnected;
            networkElement.Disconnected += OnDisconnected;
            networkElement.SocketErrored += OnSocketErrored;
            networkElement.DataReceived += NetworkInput.ProcessReceivedData;
        }

        /// <summary>
        ///     Node nickname.
        /// </summary>
        public string Nickname
        {
            get => nickname;
            internal set
            {
                var previousNickname = nickname;
                nickname = value;
                NicknameChanged?.Invoke(this, previousNickname);
            }
        }

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
        ///     Is node reconnecting.
        /// </summary>
        public bool Reconnecting { get; private set; }

        /// <summary>
        ///     Node successful connected and ready.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Trying reconnect.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        public event EventHandler HardDisconnect;

        /// <summary>
        ///     User changed nickname of node. Returns previous nickname in parameter.
        /// </summary>
        public event EventHandler<string> NicknameChanged;

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
                if (!Ready && !Reconnecting)
                {
                    NetworkElement.Close();
                }
            });
        }

        private void OnDisconnected(object sender, bool hardDisconnect)
        {
            Ready = false;

            if (hardDisconnect)
            {
                Reconnecting = false;
                HardDisconnect?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Reconnecting = true;
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
            Encryption.ImportPublicKey(handshake.PublicKey);
            NetworkOutput.SendKey();
        }

        private void OnKeyInfoReceived(object sender, KeyInfo e)
        {
            Encryption.ImportAesKey(e);
            Ready = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        internal void Dispose()
        {
            NetworkElement.Close();
            Encryption.Dispose();
        }
    }
}