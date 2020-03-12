using Lanchat.Common.Types;
using System;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
{
    internal class RecievedHandshakeEventArgs : EventArgs
    {
        internal Handshake NodeHandshake { get; set; }
    }
}
