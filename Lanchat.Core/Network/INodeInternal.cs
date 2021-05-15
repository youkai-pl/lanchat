using System;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal interface INodeInternal
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