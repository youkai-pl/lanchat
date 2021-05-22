using System;

namespace Lanchat.Core.Network
{
    internal interface INodeInternal
    {
        Guid Id { get; }
        bool Ready { get; set; }
        void OnConnected();
        void OnDisconnected();
        void OnCannotConnect();
    }
}