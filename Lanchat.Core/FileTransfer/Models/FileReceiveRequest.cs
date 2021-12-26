using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.FileTransfer.Models
{
    internal class FileReceiveRequest
    {
        [Required]
        [MaxLength(46)]
        public string FileName { get; init; }
        [Required] public long PartsCount { get; init; }
    }
}