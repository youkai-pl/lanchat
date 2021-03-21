using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    public class Handshake
    {
        [Required] [MaxLength(20)] public string Nickname { get; init; }

        [Required] public Status Status { get; init; }

        [Required] public PublicKey PublicKey { get; init; }
    }
}