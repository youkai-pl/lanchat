using System;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.Node
{
    internal interface INodeInternal
    {
        string Nickname { set; }

        Status Status { set; }
        
        Guid Id { get; }

        bool Ready { get; set; }

        bool IsSession { get; }

        bool HandshakeReceived { get; set; }
        
        IModelEncryption ModelEncryption { get; }
        
        void SendHandshake();

        void OnConnected();

        void OnCannotConnect();
    }
}