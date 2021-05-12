using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Network.Models
{
    internal class NicknameUpdate
    {
        [Required] [MaxLength(20)] public string NewNickname { get; init; }
    }
}