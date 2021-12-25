using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Encryption.Models;
using Lanchat.Core.Identity;

namespace Lanchat.Core.Network.Models
{
    internal class Handshake
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9_-]+$")]
        public string Nickname { get; init; }

        [Required] public UserStatus UserStatus { get; init; }

        [Required] public PublicKey PublicKey { get; init; }
    }
}