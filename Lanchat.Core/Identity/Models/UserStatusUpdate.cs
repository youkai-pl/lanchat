using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Identity.Models
{
    internal class UserStatusUpdate
    {
        [Required] public UserStatus NewUserStatus { get; init; }
    }
}