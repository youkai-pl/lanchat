using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using System;

namespace Lanchat.Common.NetworkLib.EventsArgs
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
        /// Sender.
        /// </summary>
        public NodeInstance Node { get; set; }

        /// <summary>
        /// Message target.
        /// </summary>
        public MessageTarget Target { get; set; }
    }
}
