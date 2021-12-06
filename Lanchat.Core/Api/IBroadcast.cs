namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Send data to all nodes.
    /// </summary>
    public interface IBroadcast
    {
        /// <summary>
        ///     Broadcast message.
        /// </summary>
        /// <param name="message">Message content.</param>
        void SendMessage(string message);

        /// <summary>
        ///     Broadcast data.
        /// </summary>
        /// <param name="data">Model object.</param>
        void SendData(object data);
    }
}