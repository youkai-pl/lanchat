using Lanchat.Common.Types;
using System;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
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
        /// Message target.
        /// </summary>
        public MessageTarget Target { get; set; }
    }
}
