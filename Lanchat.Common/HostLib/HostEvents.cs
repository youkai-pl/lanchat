using Lanchat.Common.Types;
using Lanchat.Common.NetworkLib;
using System;
using System.Net;

namespace Lanchat.Common.HostLib
{
    internal class HostEvents
    {
        // Changed nickname
        internal event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        // Node connected
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        // Node connected
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        // Received message
        internal event EventHandler<ReceivedMessageEventArgs> RecievedMessage;

        // Received handshake
        internal event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        // Received hertbeat
        internal event EventHandler<ReceivedHeartbeatEventArgs> ReceivedHeartbeat;

        // Received symetric key
        internal event EventHandler<RecievedKeyEventArgs> ReceivedKey;

        // Receieved broadcast
        internal event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        // Received request
        internal event EventHandler<ReceivedRequestEventArgs> ReceivedRequest;

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

        internal virtual void OnReceivedRequest(string requestType, IPAddress senderIP)
        {
            ReceivedRequest(this, new ReceivedRequestEventArgs()
            {
                Type = requestType,
                SenderIP = senderIP
            });
        }
    }
}