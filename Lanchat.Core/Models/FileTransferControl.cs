namespace Lanchat.Core.Models
{
    internal class FileTransferControl
    {
        public FileTransferSignal FileTransferSignal { get; init; }
    }

    internal enum FileTransferSignal
    {
        Accepted,
        Rejected,
        Errored,
        Canceled,
        Finished
    }
}