using System.Text.Json.Serialization;

namespace Lanchat.Core.Config
{
    /// <summary>
    ///     Saved node info.
    /// </summary>
    public class NodeInfo
    {
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
        [JsonIgnore]
        public string PublicKey { get; set; }
    }
}