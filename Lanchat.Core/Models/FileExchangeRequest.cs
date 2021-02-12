using System.Text.Json.Serialization;

namespace Lanchat.Core.Models
{
    public class FileExchangeRequest
    {
        public RequestStatus RequestStatus { get; set; }
        
        [JsonIgnore]
        public string FilePath { get; set; }
        
        public string FileName { get; set; }
    }

    public enum RequestStatus
    {
        Sending,
        Accepted,
        Rejected
    }
}