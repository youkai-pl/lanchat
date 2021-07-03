using System;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Core.TransportLayer;
using Lanchat.Tests.Mock.FileTransfer;
using Lanchat.Tests.Mock.Identity;
using Lanchat.Tests.Mock.Tcp;

namespace Lanchat.Tests.Mock.Network
{
    internal class NodeMock : INodeInternal, INode
    {
        public NodeMock(IOutput outputMock = null)
        {
            Id = Guid.NewGuid();
            Host = new HostMock();
            Output = outputMock;
            User = new UserMock();
            Messaging = new Messaging(Output);
            var fileTransferOutput = new FileTransferOutput(Output);
            FileSender = new FileSender(fileTransferOutput, new StorageMock());
            FileReceiver = new FileReceiver(fileTransferOutput, new StorageMock());
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
        
        public Connection Connection { get; set; }
        public IUser User { get; set; }
        public IHost Host { get; set; }
        public IFileReceiver FileReceiver { get; set; }
        public IFileSender FileSender { get; set; }
        public IMessaging Messaging { get; set; }
        public IOutput Output { get; set; }
        public IEncryptionAlerts EncryptionAlerts { get; set; }

        public IInput Input { get; set; }
        public IPublicKeyEncryption PublicKeyEncryption { get; set; }
        public Guid Id { get; }
        public bool Ready { get; set; }
        
        public void Disconnect()
        {
            throw new NotImplementedException();
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

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler CannotConnect;
        public event EventHandler<SocketError> SocketErrored;
    }
}