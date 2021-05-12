using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class Handshake
    {
        [Required] [MaxLength(20)] internal string Nickname { get; init; }

        [Required] internal Status Status { get; init; }

        [Required] internal PublicKey PublicKey { get; init; }
    }
}