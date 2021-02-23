using System;

namespace Lanchat.Core
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}