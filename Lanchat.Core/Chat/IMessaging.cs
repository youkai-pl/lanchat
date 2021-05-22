using System;

namespace Lanchat.Core.Chat
{
    /// <summary>
    ///     Basic chat features.
    /// </summary>
    public interface IMessaging
    {
        /// <summary>
        ///     Message received.
        /// </summary>
        event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Private message received.
        /// </summary>
        event EventHandler<string> PrivateMessageReceived;

        /// <summary>
        ///     Send message.
        /// </summary>
        /// <param name="content">Message content.</param>
        void SendMessage(string content);

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        void SendPrivateMessage(string content);
    }
}