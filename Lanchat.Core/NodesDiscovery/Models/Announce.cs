using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.NodesDiscovery.Models
{
    internal class Announce
    {
        [Required] public string Guid { get; init; }

        [Required]
        [MaxLength(20)]
        [RegularExpression("^[a-zA-Z0-9_-]+$")]
        public string Nickname { get; init; }
    }
}