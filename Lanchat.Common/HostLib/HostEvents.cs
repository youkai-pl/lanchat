using Lanchat.Common.NetworkLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

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
        public event EventHandler<NodeConnectionStatusEvent> NodeConnected;

        public virtual void OnNodeConnected(IPAddress nodeIP)
        {
            NodeConnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = nodeIP
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEvent> NodeDisconnected;

        public virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = nodeIP
            });
        }

        // Recieved handshake event
        public event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        public virtual void OnReceivedHandshake(Handshake handshake, IPAddress senderIP)
        {
            ReceivedHandshake(this, new RecievedHandshakeEventArgs()
            {
                NodeHandshake = handshake,
                SenderIP = senderIP
            });
        }

        // Recieved symetric key event
        public event EventHandler<RecievedKeyEventArgs> ReceivedKey;
        public virtual void OnReceivedKey(string encryptedKey, IPAddress senderIP)
        {
            ReceivedKey(this, new RecievedKeyEventArgs()
            {
                Key = encryptedKey,
                SenderIP = senderIP
            });
        }

        // Recieved message event
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
