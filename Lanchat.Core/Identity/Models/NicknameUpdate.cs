using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Identity.Models
{
    internal class NicknameUpdate
    {
        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9_-]+$")]
        public string NewNickname { get; init; }
    }
}