using Lanchat.Common.NetworkLib.InternalEvents.Args;
using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;

namespace Lanchat.Common.NetworkLib.InternalEvents
{
    internal class NodeEvents
    {
        internal event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        internal event EventHandler HandshakeAccepted;

        internal event EventHandler NodeDisconnected;

        internal event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        internal event EventHandler ReceivedHeartbeat;

        internal event EventHandler<RecievedKeyEventArgs> ReceivedKey;

        internal event EventHandler<ReceivedListEventArgs> ReceivedList;

        internal event EventHandler<ReceivedMessageEventArgs> ReceivedMessage;

        internal event EventHandler StateChanged;

        internal virtual void OnChangedNickname(string newNickname)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
            });
        }

        internal virtual void OnHandshakeAccepted()
        {
            HandshakeAccepted(this, EventArgs.Empty);
        }

        internal virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, EventArgs.Empty);
        }

        internal virtual void OnReceivedHandshake(Handshake handshake)
        {
            ReceivedHandshake(this, new RecievedHandshakeEventArgs()
            {
                NodeHandshake = handshake
            });
        }

        internal virtual void OnReceivedHeartbeat()
        {
            ReceivedHeartbeat(this, EventArgs.Empty);
        }

        internal virtual void OnReceivedKey(Key key)
        {
            ReceivedKey(this, new RecievedKeyEventArgs()
            {
                AesKey = key.AesKey,
                AesIV = key.AesIV,
            });
        }

        internal virtual void OnReceivedList(List<ListItem> list, IPAddress localAddress)
        {
            ReceivedList(this, new ReceivedListEventArgs()
            {
                List = list,
                LocalAddress = localAddress
            });
        }

        internal virtual void OnReceivedMessage(string content)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
            });
        }

        internal virtual void OnReceivedPrivateMessage(string content)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                Private = true
            });
        }

        internal virtual void OnStateChange()
        {
            StateChanged(this, EventArgs.Empty);
        }
    }
}