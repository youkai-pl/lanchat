using System.Collections.Generic;
using System.Net;

namespace Lanchat.Core.Filesystem
{
    /// <summary>
    ///     RSA keys storage.
    /// </summary>
    public interface INodesDatabase
    {
        /// <summary>
        ///     List of saved nodes.
        /// </summary>
        List<INodeInfo> SavedNodes { get; }

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
        /// <remarks>
        ///     Creates new entry if address is not found.
        /// </remarks>
        /// <param name="ipAddress">Node IP Address</param>
        INodeInfo GetNodeInfo(IPAddress ipAddress);

         /// <summary>
        ///     Get node info by ID.
        /// </summary>
        /// <param name="id">Node ID</param>
        INodeInfo GetNodeInfo(int id);
    }
}