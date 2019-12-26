using Lanchat.Common.HostLib.Types;
using Lanchat.Common.NetworkLib;
using System;
using System.Net;

namespace Lanchat.Common.HostLib
{
    public class HostEvents
    {
        // Recieved broadcast event
        public event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        public virtual void OnReceivedBroadcast(Paperplane sender, IPAddress senderIP)
        {
            RecievedBroadcast(this, new RecievedBroadcastEventArgs()
            {
                Sender = sender,
                SenderIP = senderIP
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        public virtual void OnNodeConnected(IPAddress nodeIP)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = nodeIP
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        public virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = nodeIP
            });
        }

        // Received handshake event
        public event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        public virtual void OnReceivedHandshake(Handshake handshake, IPAddress senderIP)
        {
            ReceivedHandshake(this, new RecievedHandshakeEventArgs()
            {
                NodeHandshake = handshake,
                SenderIP = senderIP
            });
        }

        // Received symetric key event
        public event EventHandler<RecievedKeyEventArgs> ReceivedKey;

        public virtual void OnReceivedKey(Key key, IPAddress senderIP)
        {
            ReceivedKey(this, new RecievedKeyEventArgs()
            {
                AesKey = key.AesKey,
                AesIV = key.AesIV,
                SenderIP = senderIP
            });
        }

        // Received hertbeat
        public event EventHandler<ReceivedHeartbeatEventArgs> ReceivedHeartbeat;
        public virtual void OnReceivedHeartbeat(IPAddress senderIP)
        {
            ReceivedHeartbeat(this, new ReceivedHeartbeatEventArgs()
            {
                SenderIP = senderIP
            });
        }

        // Received message event
        public event EventHandler<ReceivedMessageEventArgs> RecievedMessage;

        public virtual void OnReceivedMessage(string content, IPAddress senderIP)
        {
            RecievedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                SenderIP = senderIP
            });
        }

        // Changed nickname event
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        public virtual void OnChangedNickname(string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                SenderIP = senderIP
            });
        }
    }
}