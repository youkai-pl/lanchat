using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Reflection;
using Autofac;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Encryption.Handlers;
using Lanchat.Core.FileSystem;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network.Handlers;
using Lanchat.Core.Network.Models;
using Lanchat.Core.Tcp;
using IContainer = Autofac.IContainer;

namespace Lanchat.Core.Network
{
    internal class Node : IDisposable, INode, INodeInternal
    {
        private readonly IConfig config;
        internal IContainer Container { get; }
        private string nickname;
        private string previousNickname;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        public Node(IHost host, IConfig config)
        {
            this.config = config;
            IsSession = host.IsSession;
            Host = host;

            var builder = new ContainerBuilder();
            builder.RegisterInstance(this).As<INodeInternal>().SingleInstance();
            builder.RegisterInstance(config).As<IConfig>().SingleInstance();
            builder.RegisterInstance(host).As<IHost>().SingleInstance();
            builder.RegisterType<PublicKeyEncryption>().As<IPublicKeyEncryption>().SingleInstance();
            builder.RegisterType<SymmetricEncryption>().As<ISymmetricEncryption>().SingleInstance();
            builder.RegisterType<ModelEncryption>().As<IModelEncryption>().SingleInstance();
            builder.RegisterType<Output>().As<IOutput>().SingleInstance();
            builder.RegisterType<Storage>().As<IStorage>().SingleInstance();
            builder.RegisterType<Resolver>().SingleInstance();
            builder.RegisterType<Messaging>().SingleInstance();
            builder.RegisterType<FileTransferOutput>().SingleInstance();
            builder.RegisterType<FileReceiver>().SingleInstance();
            builder.RegisterType<FileSender>().SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.Name.EndsWith("Handler"));
            
            Container = builder.Build();
            
            FileReceiver = Container.Resolve<FileReceiver>();
            FileSender = Container.Resolve<FileSender>();
            publicKeyEncryption = Container.Resolve<IPublicKeyEncryption>();
            Output = Container.Resolve<IOutput>();
            Resolver = Container.Resolve<Resolver>();
            Messaging = Container.Resolve<Messaging>();
            
            HandlersSetup.RegisterHandlers(Resolver, Container);

            var input = new Input(Resolver);
            Host.DataReceived += input.OnDataReceived;
            Host.SocketErrored += (s, e) => SocketErrored?.Invoke(s, e);

            var connection = new Connection(this);
            connection.Initialize();
        }
        
        public void Dispose()
        {
            Container.Dispose();
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
                PublicKey = publicKeyEncryption.ExportKey()
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