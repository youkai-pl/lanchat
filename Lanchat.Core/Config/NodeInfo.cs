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
        public IPAddress IpAddress { get; set; }
        
        /// <summary>
        ///     Node ID.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        ///     Node user nickname.
        /// </summary>
        public string Nickname { get; set; }
        
        /// <summary>
        ///     Public RSA Key PEM.
        /// </summary>
        public string PublicKey { get; set; }
    }
}