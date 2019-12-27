using Newtonsoft.Json;
using System;

namespace Lanchat.Common.HostLib.Types
{
    internal class Paperplane
    {
        [JsonConstructor]
        internal Paperplane(int port, Guid id)
        {
            Port = port;
            Id = id;
        }

        [JsonProperty]
        internal Guid Id { get; set; }
        [JsonProperty]
        internal int Port { get; set; }
    }
}