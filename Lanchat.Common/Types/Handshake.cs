using Newtonsoft.Json;
using System;

namespace Lanchat.Common.Types
{
    internal class Handshake
    {
        [JsonConstructor]
        internal Handshake(string nickname, string publicKey, Guid id, int port)
        {
            Nickname = nickname;
            PublicKey = publicKey;
            Id = id;
            Port = port;
        }

        [JsonProperty]
        internal Guid Id { get; set; }

        [JsonProperty]
        internal string Nickname { get; set; }

        [JsonProperty]
        internal int Port { get; set; }

        [JsonProperty]
        internal string PublicKey { get; set; }
    }
}