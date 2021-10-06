using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption;

namespace Lanchat.Core.FileTransfer.Models
{
    internal class FilePart
    {
        [Required]
        [MaxLength(2097152)]
        [Encrypt]
        public string Data { get; init; }
    }
}