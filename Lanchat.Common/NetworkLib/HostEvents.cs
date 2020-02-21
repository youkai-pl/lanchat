using Lanchat.Common.NetworkLib;
using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    internal class HostEvents
    {
        internal event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        internal event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        internal event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        internal event EventHandler<RecievedHandshakeEventArgs> ReceivedHandshake;

        internal event EventHandler<ReceivedHeartbeatEventArgs> ReceivedHeartbeat;

        internal event EventHandler<RecievedKeyEventArgs> ReceivedKey;

        internal event EventHandler<ReceivedListEventArgs> ReceivedList;

        internal event EventHandler<ReceivedRequestEventArgs> ReceivedRequest;

        internal event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        internal event EventHandler<ReceivedMessageEventArgs> RecievedMessage;

        internal virtual void OnChangedNickname(string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                SenderIP = senderIP
            });
        }

        internal virtual void OnNodeConnected(Socket socket, IPAddress ip)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Socket = socket,
                NodeIP = ip
            });
        }

        internal virtual void OnNodeDisconnected(IPAddress nodeIP)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = nodeIP
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

        internal virtual void OnReceivedList(List<ListItem> list, IPAddress localAddress)
        {
            ReceivedList(this, new ReceivedListEventArgs()
            {
                List = list,
                LocalAddress = localAddress
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