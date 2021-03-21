namespace Lanchat.Core.Models
{
    internal class StatusUpdate
    {
        public Status NewStatus { get; set; }
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