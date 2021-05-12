using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption;

namespace Lanchat.Core.Models
{
    internal class Message
    {
        [Required] [MaxLength(1500)] [Encrypt] internal string Content { get; init; }

        internal bool Private { get; init; }
    }
}