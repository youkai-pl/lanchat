using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.NodesDiscovery.Models
{
    internal class Announce
    {
        [Required] public string Guid { get; init; }

        [Required] [MaxLength(20)] public string Nickname { get; init; }
    }
}