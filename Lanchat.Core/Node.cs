using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NetworkIO;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core
{
    /// <summary>
    ///     Connected user.
    /// </summary>
    public class Node : IDisposable, INotifyPropertyChanged, INodeState
    {
        private readonly IConfig config;

        internal readonly Encryptor Encryptor;

        /// <see cref="FileReceiver" />
        public readonly FileReceiver FileReceiver;

        /// <see cref="FileSender" />
        public readonly FileSender FileSender;

        /// <see cref="Messaging" />
        public readonly Messaging Messaging;

        /// <see cref="INetworkElement" />
        public readonly INetworkElement NetworkElement;

        internal readonly INetworkOutput NetworkOutput;
        internal readonly Resolver Resolver;

        internal bool HandshakeReceived;
        internal bool IsSession;
        private string nickname;
        private string previousNickname;
        private Status status;

        internal Node(INetworkElement networkElement, IConfig config, bool isSession)
        {
            this.config = config;
            IsSession = isSession;
            NetworkElement = networkElement;
            NetworkOutput = new NetworkOutput(NetworkElement, this);
            Encryptor = new Encryptor();
            Messaging = new Messaging(NetworkOutput, Encryptor);
            FileReceiver = new FileReceiver(NetworkOutput, Encryptor, config);
            FileSender = new FileSender(NetworkOutput, Encryptor);

            Resolver = new Resolver(this);
            var networkInput = new NetworkInput(Resolver);
            Resolver.Handlers.Add(new HandshakeHandler(this));
            Resolver.Handlers.Add(new KeyInfoHandler(this));
            Resolver.Handlers.Add(new ConnectionControlHandler(this));
            Resolver.Handlers.Add(new StatusUpdateHandler(this));
            Resolver.Handlers.Add(new NicknameUpdateHandler(this));
            Resolver.Handlers.Add(new MessageHandler(Messaging));
            Resolver.Handlers.Add(new FilePartHandler(FileReceiver));
            Resolver.Handlers.Add(new FileTransferControlHandler(FileReceiver, FileSender));

            NetworkElement.Disconnected += OnDisconnected;
            NetworkElement.DataReceived += networkInput.ProcessReceivedData;
            NetworkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            if (IsSession)
            {
                SendHandshake();
            }

            // Check is connection established successful after timeout.
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!Ready) NetworkElement.Close();
            });
        }

        /// <summary>
        ///     Node user nickname.
        /// </summary>
        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
            set
            {
                if (value == nickname) return;
                previousNickname = nickname;
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
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
        ///     Node user status.
        /// </summary>
        public Status Status
        {
            get => status;
            set
            {
                if (value == status) return;
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>
        ///     Dispose node. For safe disconnect use <see cref="Disconnect" /> instead.
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
        ///     Node successful connected and ready to data exchange.
        /// </summary>
        public event EventHandler Connected;

        /// <summary>
        ///     Node disconnected. Cannot reconnect.
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        ///     Raise when connection attempt failed.
        /// </summary>
        public event EventHandler CannotConnect;

        /// <summary>
        ///     TCP session or client returned error.
        /// </summary>
        public event EventHandler<SocketError> SocketErrored;

        /// <summary>
        ///     Disconnect from node.
        /// </summary>
        public void Disconnect()
        {
            NetworkOutput.SendSystemData(new ConnectionControl
            {
                Status = ConnectionControlStatus.RemoteClose
            });
            Dispose();
        }

        // Network elements events.
        private void OnDisconnected(object sender, EventArgs _)
        {
            if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                CannotConnect?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                Status = config.Status,
                PublicKey = Encryptor.ExportPublicKey()
            };

            NetworkOutput.SendSystemData(handshake);
        }
        
        internal void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}