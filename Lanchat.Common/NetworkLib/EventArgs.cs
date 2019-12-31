using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Changed node nickname event.
    /// </summary>
    public class ChangedNicknameEventArgs : EventArgs
    {
        /// <summary>
        /// New nickname.
        /// </summary>
        public string NewNickname { get; set; }

        /// <summary>
        /// Old nickname.
        /// </summary>
        public string OldNickname { get; set; }

        /// <summary>
        /// IP of the sending node.
        /// </summary>
        public IPAddress SenderIP { get; set; }
    }

    /// <summary>
    /// Host started.
    /// </summary>
    public class HostStartedEventArgs : EventArgs
    {
        /// <summary>
        /// Host listening port.
        /// </summary>
        public int Port { get; set; }
    }

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
    }

    /// <summary>
    /// Received message.
    /// </summary>
    public class ReceivedMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Message content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Sender nickname.
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// IP of the sending node.
        /// </summary>
        public IPAddress SenderIP { get; set; }
    }

    internal class ReceivedHeartbeatEventArgs : EventArgs
    {
        internal IPAddress SenderIP { get; set; }
    }

    internal class RecievedBroadcastEventArgs : EventArgs
    {
        internal Paperplane Sender { get; set; }
        internal IPAddress SenderIP { get; set; }
    }

    internal class RecievedHandshakeEventArgs : EventArgs
    {
        internal Handshake NodeHandshake { get; set; }
        internal IPAddress SenderIP { get; set; }
    }

    internal class RecievedKeyEventArgs : EventArgs
    {
        internal string AesIV { get; set; }
        internal string AesKey { get; set; }
        internal IPAddress SenderIP { get; set; }
    }

    internal class ReceivedRequestEventArgs : EventArgs
    {
        internal string Type { get; set; }
        internal IPAddress SenderIP { get; set; }
    }

    internal class ReceivedListEventArgs : EventArgs
    {
        internal List<ListItem> List { get; set; }
    }
}