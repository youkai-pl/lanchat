using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Chat.Models
{
    internal class UserStatusUpdate
    {
        [Required] public UserStatus NewUserStatus { get; init; }
    }
}