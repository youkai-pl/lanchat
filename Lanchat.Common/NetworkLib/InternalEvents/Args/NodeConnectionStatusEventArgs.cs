using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
{
    /// <summary>
    /// Node connection status.
    /// </summary>
    public class NodeConnectionStatusEventArgs : EventArgs
    {
        /// <summary>
        /// Node nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Node ip.
        /// </summary>
        public IPAddress NodeIP { get; set; }

        internal Socket Socket { get; set; }
    }
}
