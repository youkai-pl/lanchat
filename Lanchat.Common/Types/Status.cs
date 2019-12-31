namespace Lanchat.Common.Types
{
    /// <summary>
    /// Possible node states
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Waiting for handshake and key exchange.
        /// </summary>
        Waiting,

        /// <summary>
        /// Ready to use.
        /// </summary>
        Ready,

        /// <summary>
        /// Doesn't sends heartbeat.
        /// </summary>
        Suspended,

        /// <summary>
        /// Resumed after suspend.
        /// </summary>
        Resumed,

        /// <summary>
        /// Connection failed. Auto-connect blocked.
        /// </summary>
        Failed
    }
}
