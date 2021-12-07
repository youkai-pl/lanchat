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
        /// <remakrs>
        ///     Sending messages to all users is handled by <see cref="Api.IBroadcast"/>
        /// </remakrs>
        event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Private message received.
        /// </summary>
        event EventHandler<string> PrivateMessageReceived;

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        void SendPrivateMessage(string content);

        internal void SendMessage(string content);
    }
}