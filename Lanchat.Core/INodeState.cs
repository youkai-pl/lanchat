using System;

namespace Lanchat.Core
{
    public interface INodeState
    {
        bool Ready { get; }
        Guid Id { get; }
    }
}