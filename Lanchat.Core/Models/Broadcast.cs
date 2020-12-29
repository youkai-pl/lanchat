using System.Net;
using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    public class Broadcast
    {
        public string Guid { get; set; }
        public string Nickname { get; set; }

        [JsonIgnore] public IPAddress IpAddress { get; set; }
    }
}