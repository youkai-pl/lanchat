using Lanchat.Common.HostLib;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public class RecievedBroadcastEventArgs : EventArgs
    {
        public Paperplane Sender { get; set; }
        public IPAddress SenderIP { get; set; }
    }

    public class NodeConnectionStatusEvent : EventArgs
    {
        public IPAddress NodeIP { get; set; }
        public string Nickname { get; set; }
    }

    public class RecievedHandshakeEventArgs : EventArgs
    {
        public Handshake NodeHandshake { get; set; }
        public IPAddress SenderIP { get; set; }
    }

    public class RecievedMessageEventArgs : EventArgs
    {
        public string Content { get; set; }
        public string Nickname { get; set; }
        public IPAddress SenderIP { get; set; }
    }

    public class ChangedNicknameEventArgs : EventArgs
    {
        public string NewNickname { get; set; }
        public string OldNickname { get; set; }
        public IPAddress SenderIP { get; set; }
    }
}