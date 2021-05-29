using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Encryption.Models
{
    internal class PublicKey
    {
        [Required] public byte[] RsaModulus { get; init; }
        [Required] public byte[] RsaExponent { get; init; }
    }
}