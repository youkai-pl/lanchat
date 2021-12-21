using Lanchat.Core.Network;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.NodesDiscovery
{
    internal interface INodesExchange
    {
        void ConnectWithList(INode sender, NodesList nodesList);
        void ConnectWithAwaitingList(INode node);
    }
}