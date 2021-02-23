namespace Lanchat.Core.Models
{
    public class FileTransferControl
    {
        public RequestStatus RequestStatus { get; init; }
        public string FileName { get; init; }
        public long Parts { get; init; }
    }

    public enum RequestStatus
    {
        Sending,
        Accepted,
        Rejected,
        Errored,
        Canceled
    }
}