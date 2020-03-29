using Lanchat.Common.Types;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib.EventsArgs
{
    internal class RecievedBroadcastEventArgs : EventArgs
    {
        internal Paperplane Sender { get; set; }
        internal IPAddress SenderIP { get; set; }
    }
}