using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption;

namespace Lanchat.Core.Models
{
    internal class FilePart
    {
        [MaxLength(1398102)] [Encrypt] public string Data { get; init; }

        public bool Last { get; set; }
    }
}