using Newtonsoft.Json;
using System;

namespace Lanchat.Common.Types
{
    /// <summary>
    /// Hansshake.
    /// </summary>
    public class Handshake
    {
        /// <summary>
        /// Handshake constructor.
        /// </summary>
        /// <param name="nickname">Node nickname</param>
        /// <param name="publicKey">Public RSA key</param>
        /// <param name="port">Node host port</param>
        [JsonConstructor]
        public Handshake(string nickname, string publicKey, int port)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Port = port;
        }

        /// <summary>
        /// Node nickname.
        /// </summary>
        [JsonProperty]
        public string Nickname { get; set; }

        /// <summary>
        /// Node host port.
        /// </summary>
        [JsonProperty]
        public int Port { get; set; }

        /// <summary>
        /// Node host port.
        /// </summary>
        [JsonProperty]
        public string PublicKey { get; set; }
    }
}