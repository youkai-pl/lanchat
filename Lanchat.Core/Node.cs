using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Node
    {
        public readonly NetworkIO NetworkIO;
        internal readonly INetworkElement NetworkElement;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        public Node(INetworkElement networkElement)
        {
            NetworkElement = networkElement;
            NetworkIO = new NetworkIO(this);
            networkElement.Connected += OnConnected;
            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += OnDataReceived;
            networkElement.SocketErrored += OnSocketErrored;
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

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Ping received.
        /// </summary>
        public event EventHandler PingReceived;

        internal event EventHandler<IPAddress> NodeInfoReceived;

        // Events handlers
        private void OnConnected(object sender, EventArgs e)
        {
            NetworkIO.SendHandshake();

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

        private void OnDataReceived(object sender, string e)
        {
            try
            {
                var data = NetworkIO.DeserializeInput(e);

                // If node isn't ready ignore every messages except handshake
                if (!Ready && data.Type != DataTypes.Handshake)
                {
                    return;
                }

                switch (data.Type)
                {
                    case DataTypes.Message:
                        var message = JsonSerializer.Deserialize<Message>(data.Data.ToString());
                        MessageReceived?.Invoke(this, message.Content);
                        break;

                    case DataTypes.Ping:
                        PingReceived?.Invoke(this, EventArgs.Empty);
                        break;

                    case DataTypes.Handshake:
                        var handshake = JsonSerializer.Deserialize<Handshake>(data.Data.ToString());
                        Nickname = handshake.Nickname;
                        Ready = true;
                        Connected?.Invoke(this, EventArgs.Empty);
                        break;

                    case DataTypes.NewNode:
                        if (IPAddress.TryParse(data.Data.ToString(), out var ipAddress))
                        {
                            NodeInfoReceived?.Invoke(this, ipAddress);
                        }

                        break;

                    default:
                        Debug.WriteLine("Unknown type received");
                        break;
                }
            }

            // Input errors catching
            catch (Exception ex)
            {
                if (ex is JsonException || ex is ArgumentNullException)
                {
                    Debug.WriteLine("Invalid json received");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}