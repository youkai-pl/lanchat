using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    public class NetworkEvents
    {
        // Host started
        public event EventHandler<HostStartedEventArgs> HostStarted;
        public virtual void OnHostStarted(int port)
        {
            HostStarted(this, new HostStartedEventArgs()
            {
                Port = port
            });
        }

        // Received message event
        public event EventHandler<ReceivedMessageEventArgs> ReceivedMessage;

        public virtual void OnReceivedMessage(string content, string nickname)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                Nickname = nickname
            });
        }

        // Node connected event
        public event EventHandler<NodeConnectionStatusEvent> NodeConnected;

        public virtual void OnNodeConnected(IPAddress ip, string nickname)
        {
            NodeConnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        // Node disconnected event
        public event EventHandler<NodeConnectionStatusEvent> NodeDisconnected;

        public virtual void OnNodeDisconnected(IPAddress ip, string nickname)
        {
            NodeDisconnected(this, new NodeConnectionStatusEvent()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        // Changed nickname event
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        public virtual void OnChangedNickname(string oldNickname, string newNickname, IPAddress senderIP)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                OldNickname = oldNickname,
                SenderIP = senderIP
            });
        }
    }
}