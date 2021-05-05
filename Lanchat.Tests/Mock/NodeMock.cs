using System;
using System.ComponentModel;
using System.Net.Sockets;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.FileTransfer;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.Tcp;
using Lanchat.Tests.Mock.Encryption;

namespace Lanchat.Tests.Mock
{
    internal class NodeMock : INodeInternal, INode
    {
        public NodeMock(IOutput outputMock = null)
        {
            Host = null;
            ModelEncryption = new ModelEncryptionMock();
            Output = outputMock;
            Messaging = new Messaging(Output);
        }

        public string ShortId { get; }
        public bool Ready { get; set; }
        public bool IsSession => false;
        public IHost Host { get; }
        public Messaging Messaging { get; }
        public FileReceiver FileReceiver { get; }
        public FileSender FileSender { get; }
        public IOutput Output { get; }
        public IResolver Resolver { get; }
        public string Nickname { get; set; } = "Nickname";
        public string PreviousNickname { get; }
        public Status Status { get; set; } = Status.Online;
        public Guid Id { get; } = Guid.NewGuid();
        
        public bool HandshakeSent { get; private set; }
        public IModelEncryption ModelEncryption { get; }
        public bool ConnectedEvent { get; private set; }
        public bool CannotConnectEvent { get; private set; }
        
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<SocketError> SocketErrored;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void SendHandshake()
        {
            HandshakeSent = true;
        }
        
        public void Disconnect()
        {
            throw new NotImplementedException();
        }
        
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