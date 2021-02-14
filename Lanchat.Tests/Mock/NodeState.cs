using Lanchat.Core;

namespace Lanchat.Tests.Mock
{
    public class NodeState : INodeState
    {
        public bool Ready { get; } = true;
    }
}