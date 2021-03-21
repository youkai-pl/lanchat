using System;

namespace Lanchat.Core.Handlers
{
    internal interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}