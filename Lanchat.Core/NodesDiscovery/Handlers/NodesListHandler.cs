using Lanchat.Core.Api;
using Lanchat.Core.Network;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.NodesDiscovery.Handlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly INodesExchange nodesExchange;
        private readonly INode node;

        public NodesListHandler(INode node, INodesExchange nodesExchange)
        {
            this.nodesExchange = nodesExchange;
            this.node = node;
        }

        protected override void Handle(NodesList nodesList)
        {
            Disabled = true;
            nodesExchange.ConnectWithList(node, nodesList);
        }
    }
}