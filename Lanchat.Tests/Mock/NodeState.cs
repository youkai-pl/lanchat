using System;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock
{
    public class NodeState : INodeInternals
    {
        public bool Ready { get; set; } = true;
        public bool IsSession { get; }
        public bool HandshakeReceived { get; set; }
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

        public string Nickname { get; set; }
        public Status Status { get; set; }
        public Guid Id { get; } = Guid.NewGuid();
    }
}