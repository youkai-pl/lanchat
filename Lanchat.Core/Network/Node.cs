using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal class Node : IDisposable, INode, INodeInternal
    {
        private readonly IConfig config;
        private string nickname;
        private string previousNickname;

        internal Node(IHost host, IConfig config)
        {
            this.config = config;
            IsSession = host.IsSession;
            Host = host;
            PublicKeyEncryption = new PublicKeyEncryption();
            SymmetricEncryption = new SymmetricEncryption(PublicKeyEncryption);
            ModelEncryption = new ModelEncryption(SymmetricEncryption);
            Output = new Output(Host, this);
            Messaging = new Messaging(Output);

            var fileTransferOutput = new FileTransferOutput(Output);
            var fileSystem = new Storage(config);
            FileReceiver = new FileReceiver(fileTransferOutput, fileSystem);
            FileSender = new FileSender(fileTransferOutput, fileSystem);

            Resolver = new Resolver(this);
            HandlersSetup.RegisterHandlers(Resolver, this, fileSystem);

            var input = new Input(Resolver);
            Host.DataReceived += input.OnDataReceived;
            Host.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            var connection = new Connection(this);
            connection.Initialize();
        }

        internal ISymmetricEncryption SymmetricEncryption { get; }
        internal IPublicKeyEncryption PublicKeyEncryption { get; }

        public void Dispose()
        {
            Host.Close();
            FileSender.Dispose();
            FileReceiver.Dispose();
            PublicKeyEncryption.Dispose();
            GC.SuppressFinalize(this);
        }

        public IResolver Resolver { get; }
        public FileReceiver FileReceiver { get; }
        public FileSender FileSender { get; }
        public Messaging Messaging { get; }
        public IHost Host { get; }
        public IOutput Output { get; }

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
        public Guid Id => Host.Id;
        public bool Ready { get; set; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Disconnect()
        {
            Output.SendPrivilegedData(new ConnectionControl
            {
                Status = ConnectionStatus.RemoteDisconnect
            });
            Dispose();
        }

        public IModelEncryption ModelEncryption { get; }
        public bool IsSession { get; }

        public void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                UserStatus = config.UserStatus,
                PublicKey = PublicKeyEncryption.ExportKey()
            };

            Output.SendPrivilegedData(handshake);
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

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}