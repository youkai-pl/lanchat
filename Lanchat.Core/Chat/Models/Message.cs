using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption;

namespace Lanchat.Core.Chat.Models
{
    internal class Message
    {
        [Required] [MaxLength(1500)] [Encrypt] public string Content { get; init; }

        [Required] public bool Private { get; init; }
    }
}