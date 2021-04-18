using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.Node
{
    internal class NodeImplementation : IDisposable, INodePublic, INodeInternal
    {
        private readonly IConfig config;
        private readonly IPublicKeyEncryption publicKeyEncryption;
        private string nickname;
        private string previousNickname;
        private Status status;

        internal NodeImplementation(INetworkElement networkElement, IConfig config)
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

        public void Dispose()
        {
            NetworkElement.Close();
            FileSender.Dispose();
            FileReceiver.CancelReceive();
            publicKeyEncryption.Dispose();
            GC.SuppressFinalize(this);
        }

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

        public void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }

        public Resolver Resolver { get; }
        public FileReceiver FileReceiver { get; }
        public FileSender FileSender { get; }
        public Messaging Messaging { get; }
        public INetworkElement NetworkElement { get; }
        public IOutput Output { get; }

        public bool IsSession { get; }
        public bool HandshakeReceived { get; set; }
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

        public string PreviousNickname => $"{previousNickname}#{ShortId}";
        public string ShortId => Id.GetHashCode().ToString().Substring(1, 4);

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

        public Guid Id => NetworkElement.Id;
        public bool Ready { get; set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;

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