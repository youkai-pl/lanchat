using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption;

namespace Lanchat.Core.FileTransfer.Models
{
    internal class FilePart
    {
        [Required]
        [MaxLength(1398102)]
        [Encrypt]
        public string Data { get; init; }
    }
}