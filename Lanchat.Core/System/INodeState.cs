using System;

namespace Lanchat.Core.System
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}