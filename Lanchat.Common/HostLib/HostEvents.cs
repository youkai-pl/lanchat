using Lanchat.Common.HostLib.Types;
using Lanchat.Common.NetworkLib;
using System;
using System.Net;

namespace Lanchat.Common.HostLib
{
    internal class HostEvents
    {
        // Changed nickname event
        internal event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        // Node connected event
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        // Node connected event
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        // Received message event
        internal event EventHandler<ReceivedMessageEventArgs> RecievedMessage;

        // Received handshake event
        internal event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        // Received hertbeat
        internal event EventHandler<ReceivedHeartbeatEventArgs> ReceivedHeartbeat;

        // Received symetric key event
        internal event EventHandler<RecievedKeyEventArgs> ReceivedKey;

        // Recieved broadcast event
        internal event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        internal virtual void OnChangedNickname(string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                SenderIP = senderIP
            });
        }

        internal virtual void OnNodeConnected(IPAddress nodeIP)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = nodeIP
            });
        }

        internal virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = nodeIP
            });
        }

        internal virtual void OnReceivedHandshake(Handshake handshake, IPAddress senderIP)
        {
            ReceivedHandshake(this, new RecievedHandshakeEventArgs()
            {
                NodeHandshake = handshake,
                SenderIP = senderIP
            });
        }

        internal virtual void OnReceivedHeartbeat(IPAddress senderIP)
        {
            ReceivedHeartbeat(this, new ReceivedHeartbeatEventArgs()
            {
                SenderIP = senderIP
            });
        }

        internal virtual void OnReceivedKey(Key key, IPAddress senderIP)
        {
            ReceivedKey(this, new RecievedKeyEventArgs()
            {
                AesKey = key.AesKey,
                AesIV = key.AesIV,
                SenderIP = senderIP
            });
        }

        internal virtual void OnReceivedMessage(string content, IPAddress senderIP)
        {
            RecievedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                SenderIP = senderIP
            });
        }

        internal virtual void OnReceivedBroadcast(Paperplane sender, IPAddress senderIP)
        {
            RecievedBroadcast(this, new RecievedBroadcastEventArgs()
            {
                Sender = sender,
                SenderIP = senderIP
            });
        }
    }
}