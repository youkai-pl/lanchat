using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    public class FileExchangeRequest
    {
        public string Checksum { get; set; }
        public RequestStatus RequestStatus { get; set; }
        
        [JsonIgnore]
        public string FilePath { get; set; }
    }

    public enum RequestStatus
    {
        Sending,
        Accepted,
        Rejected
    }
}