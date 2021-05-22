using System;

namespace Lanchat.Core.Network
{
    internal interface INodeInternal
    {
        string Nickname { set; }
        Guid Id { get; }
        bool Ready { get; set; }
        void OnConnected();
        void OnDisconnected();
        void OnCannotConnect();
    }
}