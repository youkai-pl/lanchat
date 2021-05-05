using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.Node
{
    internal class LocalNode : IDisposable, INode, INodeInternal
    {
        private readonly IConfig config;
        private string nickname;
        private string previousNickname;
        private Status status;
        
        internal LocalNode(INetworkElement networkElement, IConfig config)
        {
            this.config = config;
            IsSession = networkElement.IsSession;
            NetworkElement = networkElement;
            PublicKeyEncryption = new PublicKeyEncryption();
            SymmetricEncryption = new SymmetricEncryption(PublicKeyEncryption);
            ModelEncryption = new ModelEncryption(SymmetricEncryption);
            Output = new Output(NetworkElement, this);
            Messaging = new Messaging(Output);
            FileReceiver = new FileReceiver(Output, config);
            FileSender = new FileSender(Output);
            Resolver = new Resolver(this);
            var input = new Input(Resolver);
            
            HandlersSetup.RegisterHandlers(Resolver, this);
            
            NetworkElement.DataReceived += input.OnDataReceived;
            NetworkElement.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            var connection = new Connection(this);
            connection.Initialize();
        }

        public IResolver Resolver { get; }
        public IModelEncryption ModelEncryption { get; }
        public FileReceiver FileReceiver { get; }
        public FileSender FileSender { get; }
        public Messaging Messaging { get; }
        public INetworkElement NetworkElement { get; }
        public IOutput Output { get; }
        public bool IsSession { get; }
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

        public void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                Status = config.Status,
                PublicKey = PublicKeyEncryption.ExportKey()
            };

            Output.SendPrivilegedData(handshake);
        }
        
        public void Disconnect()
        {
            Output.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionControlStatus.RemoteClose
            });
            Dispose();
        }
        
        public void Dispose()
        {
            NetworkElement.Close();
            FileSender.Dispose();
            FileReceiver.CancelReceive();
            PublicKeyEncryption.Dispose();
            GC.SuppressFinalize(this);
        }
        
        public void OnConnected()
        {
            Connected?.Invoke(this, EventArgs.Empty);
        }

        public void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
        
        public void OnCannotConnect()
        {
            CannotConnect?.Invoke(this, EventArgs.Empty);
        }

        internal event EventHandler CannotConnect;
        internal ISymmetricEncryption SymmetricEncryption { get; }
        internal IPublicKeyEncryption PublicKeyEncryption { get; }
        
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}