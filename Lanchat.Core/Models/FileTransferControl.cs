using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class FileTransferControl
    {
        [Required] public FileTransferStatus Status { get; init; }
    }

    internal enum FileTransferStatus
    {
        Accepted,
        Rejected,
        ReceiverError,
        SenderError,
        Finished
    }
}