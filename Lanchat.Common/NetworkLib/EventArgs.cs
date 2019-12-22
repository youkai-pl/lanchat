using Lanchat.Common.HostLib.Types;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public class HostStartedEventArgs : EventArgs
    {
        public int Port { get; set; }
    }

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

    public class RecievedKeyEventArgs : EventArgs
    {
        public string AesKey { get; set; }
        public string AesIV { get; set; }

        public IPAddress SenderIP { get; set; }
    }

    public class ReceivedHeartbeatEventArgs : EventArgs
    {
        public IPAddress SenderIP { get; set; }
    }

    public class ReceivedMessageEventArgs : EventArgs
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