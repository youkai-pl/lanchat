using System;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    /// <summary>
    /// Network API inputs class.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Check <see cref="HostStartedEventArgs"/>
        /// </summary>
        public event EventHandler<HostStartedEventArgs> HostStarted;

        /// <summary>
        /// Host properly started event.
        /// </summary>
        /// <param name="port">Host listen port</param>
        public virtual void OnHostStarted(int port)
        {
            HostStarted(this, new HostStartedEventArgs()
            {
                Port = port
            });
        }

        /// <summary>
        /// Check <see cref="ReceivedMessageEventArgs"/>
        /// </summary>
        public event EventHandler<ReceivedMessageEventArgs> ReceivedMessage;

        /// <summary>
        /// Received message event.
        /// </summary>
        /// <param name="content">Message content</param>
        /// <param name="nickname">Sender nickname</param>
        public virtual void OnReceivedMessage(string content, string nickname)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                Nickname = nickname
            });
        }

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        /// <summary>
        /// Node connected event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeConnected(IPAddress ip, string nickname)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        /// <summary>
        /// Node disconnected event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeDisconnected(IPAddress ip, string nickname)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeSuspended;

        /// <summary>
        /// Node suspended event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeSuspended(IPAddress ip, string nickname)
        {
            NodeSuspended(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeResumed;

        /// <summary>
        /// Node resumed event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeResumed(IPAddress ip, string nickname)
        {
            NodeResumed(this, new NodeConnectionStatusEventArgs()
            {
                NodeIP = ip,
                Nickname = nickname
            });
        }

        /// <summary>
        /// Check <see cref="ChangedNicknameEventArgs"/>
        /// </summary>
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        /// <summary>
        /// Node nickname change event.
        /// </summary>
        /// <param name="oldNickname">Old node nickname</param>
        /// <param name="newNickname">New node nickname</param>
        /// <param name="senderIP">Node ip</param>
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