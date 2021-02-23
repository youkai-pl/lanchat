using System;

namespace Lanchat.Core.Connection
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}