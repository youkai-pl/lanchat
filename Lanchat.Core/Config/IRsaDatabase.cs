using System.Net;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     RSA keys storage.
    /// </summary>
    public interface IRsaDatabase
    {
        /// <summary>
        ///     Get local public and private keys.
        /// </summary>
        /// <returns>PEM file string</returns>
        string GetLocalPem();

        /// <summary>
        ///     Save local public and private keys.
        /// </summary>
        /// <param name="pem">PEM file string</param>
        void SaveLocalPem(string pem);

        /// <summary>
        ///     Get node public key by IP Address.
        /// </summary>
        /// <param name="ipAddress">Node IP Address</param>
        string GetNodePem(IPAddress ipAddress);

        /// <summary>
        ///     Save node public key.
        /// </summary>
        /// <param name="ipAddress">Node IP Address.</param>
        /// <param name="pem">PEM file string.</param>
        void SaveNodePem(IPAddress ipAddress, string pem);
    }
}