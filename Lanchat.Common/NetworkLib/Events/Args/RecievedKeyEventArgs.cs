using System;

namespace Lanchat.Common.NetworkLib.Events.Args
{
    internal class RecievedKeyEventArgs : EventArgs
    {
        internal string AesIV { get; set; }
        internal string AesKey { get; set; }
    }
}
