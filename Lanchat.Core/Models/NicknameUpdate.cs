using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class NicknameUpdate
    {
        [MaxLength(20)] internal string NewNickname { get; init; }
    }
}