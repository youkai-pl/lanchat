using System;

namespace Lanchat.Core.P2P.NodeHandlers
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}