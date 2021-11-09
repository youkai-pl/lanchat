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
        NodeInfo GetNodeInfo(IPAddress ipAddress);

        /// <summary>
        ///     Update node nickname.
        /// </summary>
        /// <param name="ipAddress">Node IP Address.</param>
        /// <param name="nickname">New node nickname</param>
        void UpdateNodeNickname(IPAddress ipAddress, string nickname);
        
        /// <summary>
        ///     Update node public key.
        /// </summary>
        /// <param name="ipAddress">Node IP Address.</param>
        /// <param name="publicKey">New node public key</param>
        void UpdateNodePublicKey(IPAddress ipAddress, string publicKey);
    }
}