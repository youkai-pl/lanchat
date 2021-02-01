namespace Lanchat.Core.Models
{
    public class FileExchangeRequest
    {
        public string Checksum { get; set; }
        public FileExchangeRequestType RequestType { get; set; }
    }

    public enum FileExchangeRequestType
    {
        Sending,
        Accepted,
        Rejected
    }
}