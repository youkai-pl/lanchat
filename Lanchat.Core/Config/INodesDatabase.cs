using System.Net;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     RSA keys storage.
    /// </summary>
    public interface INodesDatabase
    {
        /// <summary>
        ///     Get local public and private keys.
        /// </summary>
        /// <returns>PEM file string</returns>
        string GetLocalNodeInfo();

        /// <summary>
        ///     Save local public and private keys.
        /// </summary>
        /// <param name="pem">PEM file string</param>
        void SaveLocalNodeInfo(string pem);

        /// <summary>
        ///     Get node info by IP address.
        /// </summary>
        /// <param name="ipAddress">Node IP Address</param>
        INodeInfo GetNodeInfo(IPAddress ipAddress);
    }
}