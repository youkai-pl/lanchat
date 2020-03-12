using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
{
    internal class ReceivedListEventArgs : EventArgs
    {
        internal List<ListItem> List { get; set; }
        internal IPAddress LocalAddress { get; set; }
    }
}
