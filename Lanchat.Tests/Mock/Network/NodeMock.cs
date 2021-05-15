using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Network;
using Lanchat.Core.Tcp;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.FileTransfer;
using Lanchat.Tests.Mock.Tcp;

namespace Lanchat.Tests.Mock.Network
{
    internal class NodeMock : INodeInternal, INode
    {
        public NodeMock(IOutput outputMock = null)
        {
            Host = new HostMock();
            Output = outputMock;
            ShortId = "9999";
            Nickname = "test";
            PreviousNickname = "test";
            ModelEncryption = new ModelEncryptionMock();
            Resolver = new Resolver(this, ModelEncryption, new List<IApiHandler>());
            Messaging = new Messaging(Output);
            
            var fileTransferOutput = new FileTransferOutput(Output);
            FileSender = new FileSender(fileTransferOutput, new StorageMock());
            FileReceiver = new FileReceiver(fileTransferOutput, new StorageMock());
        }

        public bool ConnectedEvent { get; private set; }
        public bool CannotConnectEvent { get; private set; }

        public string ShortId { get; }
        public IMessaging Messaging { get; }
        public IFileReceiver FileReceiver { get; }
        public IFileSender FileSender { get; }
        public IOutput Output { get; }
        public IResolver Resolver { get; }
        public string PreviousNickname { get; }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool Ready { get; set; }
        public bool IsSession => false;
        public IHost Host { get; }
        public string Nickname { get; set; }
        public Guid Id { get; } = Guid.NewGuid();
        public IModelEncryption ModelEncryption { get; }

        public void OnConnected()
        {
            ConnectedEvent = true;
        }

        public void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void OnCannotConnect()
        {
            CannotConnectEvent = true;
        }
    }
}