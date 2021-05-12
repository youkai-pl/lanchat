using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Encryption
{
    internal class PublicKey
    {
        [Required] public byte[] RsaModulus { get; init; }
        [Required] public byte[] RsaExponent { get; init; }
    }
}