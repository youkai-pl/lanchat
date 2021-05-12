using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class Announce
    {
        [Required] internal string Guid { get; init; }

        [Required] [MaxLength(20)] internal string Nickname { get; init; }
    }
}