using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.Node;
using Lanchat.Tests.Mock.Encryption;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Lanchat.Tests.Mock
{
    internal class NodeMock : INodeInternal
    {
        public NodeMock()
        {
            NetworkElement = null;
            ModelEncryption = new ModelEncryptionMock();
        }
        
        public bool Ready { get; set; }
        public bool IsSession => false;
        public INetworkElement NetworkElement { get; }
        public string Nickname { get; set; } = "Nickname";
        public Status Status { get; set; } = Status.Online;
        public Guid Id { get; } = Guid.NewGuid();
        
        public bool HandshakeSent { get; private set; }

        public IModelEncryption ModelEncryption { get; }
        public bool ConnectedEvent { get; private set; }
        public bool CannotConnectEvent { get; private set; }
        
        public void SendHandshake()
        {
            HandshakeSent = true;
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