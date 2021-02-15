using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    public class Node : IDisposable, INotifyPropertyChanged, INodeState, IApiHandler
    {
        public readonly Messaging Messaging;
        public readonly FileReceiver FileReceiver;
        public readonly FileSender FileSender;
        public readonly Echo Echo;
        
        internal readonly INetworkElement NetworkElement;
        internal readonly NetworkInput NetworkInput;
        internal readonly NetworkOutput NetworkOutput;
        internal readonly FileTransferHandler FileTransferHandler;
        internal readonly Encryptor Encryptor;

        private readonly IPEndPoint firstEndPoint;
        private string nickname;
        private string previousNickname;
        private Status status;
        private bool underReconnecting;

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
            Encryptor = new Encryptor();
            Messaging = new Messaging(NetworkOutput, Encryptor);
            NetworkInput = new NetworkInput(this);
            Echo = new Echo(NetworkOutput);
            FileReceiver = new FileReceiver(NetworkOutput, Encryptor);
            FileSender = new FileSender(NetworkOutput, Encryptor);
            FileTransferHandler = new FileTransferHandler(FileReceiver, FileSender);

            networkElement.Disconnected += OnDisconnected;
            networkElement.DataReceived += NetworkInput.ProcessReceivedData;
            networkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);
            
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
        public bool Ready { get; internal set; }

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

        private void SendHandshakeAndWait()
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

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal virtual void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Goodbye,
            DataTypes.KeyInfo,
            DataTypes.NodesList,
            DataTypes.StatusUpdate,
            DataTypes.Handshake,
            DataTypes.NicknameUpdate
        };
        
        public void Handle(DataTypes type, string data)
        {
            if (type == DataTypes.Goodbye)
            {
                NetworkElement.EnableReconnecting = false;
                return;
            }

            if (type == DataTypes.KeyInfo)
            {
                var keyInfo = JsonSerializer.Deserialize<KeyInfo>(data, CoreConfig.JsonSerializerOptions);
                if (keyInfo == null)
                {
                    return;
                }
            
                Encryptor.ImportAesKey(keyInfo);
                Ready = true;
                OnConnected();
                return;
            }

            if (type == DataTypes.NodesList)
            {
                var stringList = JsonSerializer.Deserialize<List<string>>(data);
                var list = new List<IPAddress>();

                // Convert strings to ip addresses.
                stringList?.ForEach(x =>
                {
                    if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
                });
                
                NodesListReceived?.Invoke(this, list);
                return;
            }

            if (type == DataTypes.Handshake)
            {
                var handshake = JsonSerializer.Deserialize<Handshake>(data, CoreConfig.JsonSerializerOptions);
                if (handshake == null)
                {
                    return;
                }
            
                Nickname = handshake.Nickname.Truncate(CoreConfig.MaxNicknameLenght);
                Encryptor.ImportPublicKey(handshake.PublicKey);
                Status = handshake.Status;
                NetworkOutput.SendSystemData(DataTypes.KeyInfo, Encryptor.ExportAesKey());
                return;
            }

            if (type == DataTypes.StatusUpdate)
            {
                if (Enum.TryParse<Status>(data, out var newStatus)) Status = newStatus;
                return;
            }

            if (type == DataTypes.NicknameUpdate)
            {
                var newNickname = data;
                Nickname = newNickname.Truncate(CoreConfig.MaxNicknameLenght);
            }
        }
    }
}