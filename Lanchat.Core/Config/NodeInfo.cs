using System.Net;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     Saved node info.
    /// </summary>
    public class NodeInfo
    {
        /// <summary>
        ///     Node IP address
        /// </summary>
        public IPAddress IpAddress { get; set; }
        
        /// <summary>
        ///     Node ID
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        ///     Node user nickname
        /// </summary>
        public string Nickname { get; set; }
    }
}