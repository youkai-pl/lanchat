using System;

namespace Lanchat.Common.NetworkLib.Events.Args
{
    /// <summary>
    /// Received message.
    /// </summary>
    public class ReceivedMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Message content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Sender nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Private message.
        /// </summary>
        public bool Private { get; set; }
    }
}
