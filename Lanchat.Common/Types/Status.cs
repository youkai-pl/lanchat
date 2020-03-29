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
        /// Connection with node closed.
        /// </summary>
        Closed
    }
}