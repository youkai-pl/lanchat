using System;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
{
    internal class RecievedKeyEventArgs : EventArgs
    {
        internal string AesIV { get; set; }
        internal string AesKey { get; set; }
    }
}
