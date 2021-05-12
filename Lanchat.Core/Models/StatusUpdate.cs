using System.ComponentModel.DataAnnotations;

namespace Lanchat.Core.Models
{
    internal class StatusUpdate
    {
        [Required] internal Status NewStatus { get; init; }
    }

    /// <summary>
    ///     Node user status.
    /// </summary>
    public enum Status
    {
        /// <summary>
        ///     Online.
        /// </summary>
        Online,

        /// <summary>
        ///     Away from keyboard.
        /// </summary>
        AwayFromKeyboard,

        /// <summary>
        ///     Do not disturb.
        /// </summary>
        DoNotDisturb
    }
}