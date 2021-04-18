using System;
using Lanchat.Core;
using Lanchat.Core.Models;
using Lanchat.Core.Node;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Lanchat.Tests.Mock
{
    public class NodeMock : INodeInternal
    {
        public bool Ready { get; set; } = true;
        public bool IsSession => false;
        public bool HandshakeReceived { get; set; }
        public string Nickname { get; set; } = "Nickname";
        public Status Status { get; set; } = Status.Online;
        public Guid Id { get; } = Guid.NewGuid();
        
        public void SendHandshake()
        {
            throw new NotImplementedException();
        }

        public void OnConnected()
        {
            throw new NotImplementedException();
        }

        public void OnCannotConnect()
        {
            throw new NotImplementedException();
        }

    }
}