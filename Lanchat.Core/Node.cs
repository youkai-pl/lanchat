using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    public class Node : IDisposable, INotifyPropertyChanged, INodeState
    {
        public readonly Messaging Messaging;
        public readonly Echo Echo;
        public readonly FileReceiver FileReceiver;
        public readonly FileSender FileSender;

        internal readonly Encryptor Encryptor;
        internal readonly INetworkElement NetworkElement;
        internal readonly INetworkOutput NetworkOutput;

        private string nickname;
        private string previousNickname;
        private Status status;
        private bool underReconnecting;
        private readonly IPEndPoint firstEndPoint;
        internal readonly bool SendHandshake;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        /// <param name="sendHandshake">Send handshake immediately</param>
        public Node(INetworkElement networkElement, bool sendHandshake)
        {
            SendHandshake = sendHandshake;
            NetworkElement = networkElement;
            firstEndPoint = networkElement.Endpoint;
            NetworkOutput = new NetworkOutput(NetworkElement, this);
            Encryptor = new Encryptor();
            Messaging = new Messaging(NetworkOutput, Encryptor);
            Echo = new Echo(NetworkOutput);
            FileReceiver = new FileReceiver(NetworkOutput, Encryptor);
            FileSender = new FileSender(NetworkOutput, Encryptor);

            var networkInput = new NetworkInput(this);
            networkInput.ApiHandlers.Add(new NodeApiHandlers(this));
            networkInput.ApiHandlers.Add(Messaging);
            networkInput.ApiHandlers.Add(FileReceiver);
            networkInput.ApiHandlers.Add(Echo);
            networkInput.ApiHandlers.Add(new FileTransferHandler(FileReceiver, FileSender));

            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += networkInput.ProcessReceivedData;
            networkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            if (SendHandshake)
            {
                SendHandshakeAndWait();
            }
        }

        /// <summary>
        ///     IP address of node.
        /// </summary>
        public IPEndPoint Endpoint
        {
            get
            {
                // Return endpoint from network element.
                try
                {
                    return NetworkElement.Endpoint;
                }

                // Or from local variable if network element is disposed.
                catch (ObjectDisposedException)
                {
                    return firstEndPoint;
                }
            }
        }

        /// <summary>
        ///     Node nickname.
        /// </summary>
        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
            set
            {
                if (value == nickname) return;
                previousNickname = nickname;
                nickname = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        public string PreviousNickname => $"{previousNickname}#{ShortId}";

        /// <summary>
        ///     Short ID.
        /// </summary>
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);

        /// <summary>
        ///     User status.
        /// </summary>
        public Status Status
        {
            get => status;
            set
            {
                if (value == status) return;
                status = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Close connection with node and dispose.
        /// </summary>
        public void Dispose()
        {
            NetworkElement.Close();
            Encryptor.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     ID of TCP client or session.
        /// </summary>
        public Guid Id => NetworkElement.Id;

        /// <summary>
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        public bool Ready { get; internal set; }

        /// <summary>
        ///     Invoked for properties like nickname or status.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

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
        ///     Raise when connection try failed.
        /// </summary>
        public event EventHandler CannotConnect;

        /// <summary>
        ///     TCP session or client for this node returned error.
        /// </summary>
        public event EventHandler<SocketError> SocketErrored;

        internal event EventHandler<List<IPAddress>> NodesListReceived;


        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        public void Disconnect()
        {
            NetworkOutput.SendSystemData(DataTypes.Goodbye);
            Dispose();
        }

        // Network elements events.
        private void OnDisconnected(object sender, bool hardDisconnect)
        {
            underReconnecting = !hardDisconnect;

            // Raise event only if node was ready before.
            if (hardDisconnect && !Ready)
            {
                Trace.WriteLine($"Cannot connect {Id} / {Endpoint}");
                CannotConnect?.Invoke(this, EventArgs.Empty);
            }
            else if (hardDisconnect && Ready)
            {
                HardDisconnect?.Invoke(this, EventArgs.Empty);
            }
            else if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }

            Ready = false;
        }

        internal void SendHandshakeAndWait()
        {
            var handshake = new Handshake
            {
                Nickname = CoreConfig.Nickname,
                Status = CoreConfig.Status,
                PublicKey = Encryptor.ExportPublicKey()
            };

            NetworkOutput.SendSystemData(DataTypes.Handshake, handshake);

            // Check is connection established successful after timeout.
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!Ready && !underReconnecting) NetworkElement.Close();
            });
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        internal void OnNodesListReceived(List<IPAddress> e)
        {
            NodesListReceived?.Invoke(this, e);
        }
    }
}