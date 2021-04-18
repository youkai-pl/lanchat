using System;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    internal interface INodeInternal
    {
        string Nickname { set; }

        Status Status { set; }
        
        Guid Id { get; }

        bool Ready { get; set; }

        bool IsSession { get; }

        bool HandshakeReceived { get; set; }

        void SendHandshake();

        void OnConnected();

        void OnCannotConnect();
    }
}