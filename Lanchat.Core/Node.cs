using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    public class Node : IDisposable, INotifyPropertyChanged, INodeState
    {
        public readonly Messaging Messaging;
        public readonly FileReceiver FileReceiver;
        public readonly FileSender FileSender;
        public readonly Echo Echo;
        
        internal readonly INetworkElement NetworkElement;
        internal readonly NetworkInput NetworkInput;
        internal readonly NetworkOutput NetworkOutput;
        internal readonly FileTransferHandler FileTransferHandler;

        private readonly IPEndPoint firstEndPoint;
        private string nickname;
        private string previousNickname;
        private Status status;
        private bool underReconnecting;
        private readonly Encryptor encryptor;

        /// <summary>
        ///     Initialize node.
        /// </summary>
        /// <param name="networkElement">TCP client or session.</param>
        /// <param name="sendHandshake">Send handshake immediately</param>
        public Node(INetworkElement networkElement, bool sendHandshake)
        {
            NetworkElement = networkElement;
            firstEndPoint = networkElement.Endpoint;
            NetworkOutput = new NetworkOutput(NetworkElement, this);
            NetworkInput = new NetworkInput(this);
            encryptor = new Encryptor();
            Echo = new Echo(NetworkOutput);
            FileReceiver = new FileReceiver(NetworkOutput, encryptor);
            FileSender = new FileSender(NetworkOutput, encryptor);
            FileTransferHandler = new FileTransferHandler(FileReceiver, FileSender);
            Messaging = new Messaging(NetworkOutput, encryptor);

            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += NetworkInput.ProcessReceivedData;
            networkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            NetworkInput.HandshakeReceived += OnHandshakeReceived;
            NetworkInput.KeyInfoReceived += OnKeyInfoReceived;

            if (sendHandshake)
                SendHandshakeAndWait();
            else
                networkElement.Connected += OnConnected;
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
        ///     ID of TCP client or session.
        /// </summary>
        public Guid Id => NetworkElement.Id;

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
        ///     Node ready. If set to false node won't send or receive messages.
        /// </summary>
        public bool Ready { get; private set; }

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
            encryptor.Dispose();
            GC.SuppressFinalize(this);
        }

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

        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        public void Disconnect()
        {
            NetworkOutput.SendSystemData(DataTypes.Goodbye);
            Dispose();
        }

        // Network elements events.

        private void OnConnected(object sender, EventArgs e)
        {
            SendHandshakeAndWait();
        }

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

        private void OnHandshakeReceived(object sender, Handshake handshake)
        {
            Nickname = handshake.Nickname.Truncate(CoreConfig.MaxNicknameLenght);
            encryptor.ImportPublicKey(handshake.PublicKey);
            Status = handshake.Status;
            NetworkOutput.SendSystemData(DataTypes.KeyInfo, encryptor.ExportAesKey());
        }

        private void OnKeyInfoReceived(object sender, KeyInfo e)
        {
            encryptor.ImportAesKey(e);
            Ready = true;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void SendHandshakeAndWait()
        {
            var handshake = new Handshake
            {
                Nickname = CoreConfig.Nickname,
                Status = CoreConfig.Status,
                PublicKey = encryptor.ExportPublicKey()
            };

            NetworkOutput.SendSystemData(DataTypes.Handshake, handshake);

            // Check is connection established successful after timeout.
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!Ready && !underReconnecting) NetworkElement.Close();
            });
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}