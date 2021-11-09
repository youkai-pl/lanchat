using System.Net;
using Lanchat.Core.Config;

namespace Lanchat.ClientCore
{
    /// <inheritdoc />
    public class NodesDatabase : INodesDatabase
    {
        /// <inheritdoc />
        public NodeInfo GetNodeByIp(IPAddress ipAddress)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc />
        public void SaveNodeInfo(NodeInfo nodeInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}