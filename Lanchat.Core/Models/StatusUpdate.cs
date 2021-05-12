using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Chat;

namespace Lanchat.Core.Models
{
    internal class StatusUpdate
    {
        [Required] public UserStatus NewUserStatus { get; init; }
    }
}