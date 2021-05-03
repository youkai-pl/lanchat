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
        
        public bool Ready { get; set; } = true;
        public bool IsSession => false;
        public INetworkElement NetworkElement { get; }
        public string Nickname { get; set; } = "Nickname";
        public Status Status { get; set; } = Status.Online;
        public Guid Id { get; } = Guid.NewGuid();

        public IModelEncryption ModelEncryption { get; }
        public void SendHandshake()
        {
            throw new NotImplementedException();
        }

        public void OnConnected()
        {
            throw new NotImplementedException();
        }

        public void OnDisconnected()
        {
            throw new NotImplementedException();
        }

        public void OnCannotConnect()
        {
            throw new NotImplementedException();
        }
    }
}