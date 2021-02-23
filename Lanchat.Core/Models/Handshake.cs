using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    public class Handshake
    {
        [Required]
        [MaxLength(20)]
        public string Nickname { get; set; }
        
        [Required]
        public Status Status { get; set; }
        
        [Required]
        public PublicKey PublicKey { get; set; }
    }
}