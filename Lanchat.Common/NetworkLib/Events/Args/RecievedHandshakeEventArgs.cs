using Lanchat.Common.Types;
using System;

namespace Lanchat.Common.NetworkLib.Events.Args
{
    internal class RecievedHandshakeEventArgs : EventArgs
    {
        internal Handshake NodeHandshake { get; set; }
    }
}
