using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    public class FileExchangeRequest
    {
        public string Checksum { get; set; }
        public FileExchangeRequestType RequestType { get; set; }
        
        [JsonIgnore]
        public string FilePath { get; set; }
    }

    public enum FileExchangeRequestType
    {
        Sending,
        Accepted,
        Rejected
    }
}