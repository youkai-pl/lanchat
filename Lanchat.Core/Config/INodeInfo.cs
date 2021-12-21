using System.Net;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     Saved node info.
    /// </summary>
    public interface INodeInfo
    {
        /// <summary>
        ///     Node IP address.
        /// </summary>
        IPAddress IpAddress { get; set; }

        /// <summary>
        ///     Node ID.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        ///     Node user nickname.
        /// </summary>
        string Nickname { get; set; }

        /// <summary>
        ///     Public RSA Key PEM.
        /// </summary>
        string PublicKey { get; set; }

        /// <summary>
        ///     User verified RSA fingerprint.
        ///     Nodes list from this node will be used.
        /// </summary>
        bool Trusted {get;set;}

        /// <summary>
        ///     Node is blocked.
        /// </summary>
        bool Blocked { get; set; }
    }
}