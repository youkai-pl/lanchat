using System;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    public interface INodeInternal
    {
        string Nickname { set; }
        Guid Id { get; }
        bool Ready { get; set; }
        bool IsSession { get; }
        IHost Host { get; }
        void OnConnected();
        void OnDisconnected();
        void OnCannotConnect();
    }
}