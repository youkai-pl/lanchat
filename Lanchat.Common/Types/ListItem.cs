using Newtonsoft.Json;

namespace Lanchat.Common.Types
{
    internal class ListItem
    {
        [JsonConstructor]
        internal ListItem(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }

        [JsonProperty]
        internal string Ip { get; set; }
        
        [JsonProperty]
        internal int Port { get; set; }
    }
}
