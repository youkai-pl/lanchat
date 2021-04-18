using System;

namespace Lanchat.Core.NodeHandlers
{
    internal interface INodeState
    {
        public bool Ready { get; }
        public Guid Id { get; }
    }
}