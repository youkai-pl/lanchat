using System.Net;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     Nodes ID's and addresses storage.
    /// </summary>
    public interface INodesDatabase
    {

        /// <summary>
        ///     Search node info by IP address.
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <returns>Save node info</returns>
        NodeInfo GetNodeByIp(IPAddress ipAddress);

        /// <summary>
        ///     Save node info.
        /// </summary>
        /// <param name="nodeInfo">Node info</param>
        void SaveNodeInfo(NodeInfo nodeInfo);
    }
}