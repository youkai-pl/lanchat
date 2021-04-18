using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.API;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core
{
    /// <inheritdoc cref="Lanchat.Core.INodePublic" />
    public class Node : IDisposable, INodePublic, INodeInternal
    {
        private readonly IConfig config;
        private readonly IPublicKeyEncryption publicKeyEncryption;
        private string nickname;
        private string previousNickname;
        private Status status;

        internal Node(INetworkElement networkElement, IConfig config)
        {
            this.config = config;
            IsSession = networkElement.IsSession;
            NetworkElement = networkElement;
            publicKeyEncryption = new PublicKeyEncryption();
            var symmetricEncryption = new SymmetricEncryption(publicKeyEncryption);
            var modelEncryption = new ModelEncryption(symmetricEncryption);
            Output = new Output(NetworkElement, this, modelEncryption);
            Messaging = new Messaging(Output);
            FileReceiver = new FileReceiver(Output, config);
            FileSender = new FileSender(Output);

            Resolver = new Resolver(this, modelEncryption);
            Resolver.RegisterHandler(new HandshakeHandler(publicKeyEncryption, symmetricEncryption, Output, this));
            Resolver.RegisterHandler(new KeyInfoHandler(symmetricEncryption, this));
            Resolver.RegisterHandler(new ConnectionControlHandler(NetworkElement));
            Resolver.RegisterHandler(new StatusUpdateHandler(this));
            Resolver.RegisterHandler(new NicknameUpdateHandler(this));
            Resolver.RegisterHandler(new MessageHandler(Messaging));
            Resolver.RegisterHandler(new FilePartHandler(FileReceiver));
            Resolver.RegisterHandler(new FileTransferControlHandler(FileReceiver, FileSender));

            NetworkElement.Disconnected += OnDisconnected;
            NetworkElement.DataReceived += Resolver.OnDataReceived;
            NetworkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            if (IsSession)
            {
                SendHandshake();
            }

            CheckIsReadyAfterTimeout();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            NetworkElement.Close();
            FileSender.Dispose();
            FileReceiver.CancelReceive();
            publicKeyEncryption.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public bool IsSession { get; }

        /// <inheritdoc />
        public bool HandshakeReceived { get; set; }

        /// <inheritdoc />
        public void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                Status = config.Status,
                PublicKey = publicKeyEncryption.ExportKey()
            };

            Output.SendPrivilegedData(handshake);
        }

        /// <inheritdoc />
        public void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public Resolver Resolver { get; }

        /// <inheritdoc />
        public FileReceiver FileReceiver { get; }

        /// <inheritdoc />
        public FileSender FileSender { get; }

        /// <inheritdoc />
        public Messaging Messaging { get; }

        /// <inheritdoc />
        public INetworkElement NetworkElement { get; }

        /// <inheritdoc />
        public IOutput Output { get; }

        /// <inheritdoc cref="INodePublic.Nickname" />
        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
            set
            {
                if (value == nickname)
                {
                    return;
                }

                previousNickname = nickname;
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <inheritdoc />
        public string PreviousNickname => $"{previousNickname}#{ShortId}";

        /// <inheritdoc />
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);

        /// <inheritdoc cref="INodePublic.Status" />
        public Status Status
        {
            get => status;
            set
            {
                if (value == status)
                {
                    return;
                }

                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <inheritdoc cref="INodePublic.Id" />
        public Guid Id => NetworkElement.Id;

        /// <inheritdoc cref="INodePublic.Ready" />
        public bool Ready { get; set; }

        /// <inheritdoc />
        public event EventHandler Connected;

        /// <inheritdoc />
        public event EventHandler Disconnected;

        /// <inheritdoc />
        public event EventHandler<SocketError> SocketErrored;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        public void Disconnect()
        {
            Output.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionControlStatus.RemoteClose
            });
            Dispose();
        }

        internal event EventHandler CannotConnect;

        private void OnDisconnected(object sender, EventArgs _)
        {
            if (Ready)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnCannotConnect();
            }
        }

        private void CheckIsReadyAfterTimeout()
        {
            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!Ready)
                {
                    OnCannotConnect();
                }
            });
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}