using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class NicknameUpdate
    {
        [Required] [MaxLength(20)] public string NewNickname { get; init; }
    }
}