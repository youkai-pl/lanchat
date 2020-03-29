using Lanchat.Common.NetworkLib.Node;
using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib.EventsArgs
{
    /// <summary>
    /// Node connection status.
    /// </summary>
    public class NodeConnectionStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Node.
        /// </summary>
        public NodeInstance Node { get; set; }

        internal IPAddress Ip { get; set; }
        internal Socket Socket { get; set; }
    }
}