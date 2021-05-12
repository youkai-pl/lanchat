using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Encryption.Models
{
    internal class KeyInfo
    {
        [Required] public byte[] AesKey { get; init; }
        [Required] public byte[] AesIv { get; init; }
    }
}