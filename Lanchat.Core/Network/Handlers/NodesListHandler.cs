using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.Network.Handlers
{
    internal class NodesListHandler : ApiHandler<NodesList>
    {
        private readonly IConfig config;
        private readonly IP2P network;
        private readonly INode node;

        public NodesListHandler(IConfig config, IP2P network, INode node)
        {
            this.config = config;
            this.network = network;
            this.node = node;
        }

        protected override void Handle(NodesList nodesList)
        {
            Disabled = true;
            if (!config.ConnectToReceivedList)
            {
                return;
            }

            network.NodesDetection.AddNodesList(node, nodesList);
        }
    }
}