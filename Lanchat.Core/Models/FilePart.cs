using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class FilePart
    {
        [MaxLength(1024*1024)]
        public byte[] Data { get; init; }
        public bool Last { get; set; }
    }
}