using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using System;
using System.Net;

namespace Lanchat.Common.NetworkLib.Api
{
    /// <summary>
    /// Network API inputs class.
    /// </summary>
    public class Events
    {
        /// <summary>
        /// Check <see cref="ChangedNicknameEventArgs"/>
        /// </summary>
        public event EventHandler<ChangedNicknameEventArgs> ChangedNickname;

        /// <summary>
        /// Check <see cref="HostStartedEventArgs"/>
        /// </summary>
        public event EventHandler<HostStartedEventArgs> HostStarted;

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeDisconnected;

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeResumed;

        /// <summary>
        /// Check <see cref="NodeConnectionStatusEventArgs"/>
        /// </summary>
        public event EventHandler<NodeConnectionStatusEventArgs> NodeSuspended;

        /// <summary>
        /// Check <see cref="ReceivedMessageEventArgs"/>
        /// </summary>
        public event EventHandler<ReceivedMessageEventArgs> ReceivedMessage;

        /// <summary>
        /// Node nickname change event.
        /// </summary>
        /// <param name="oldNickname">Old node nickname</param>
        /// <param name="newNickname">New node nickname</param>
        public virtual void OnChangedNickname(string oldNickname, string newNickname)
        {
            ChangedNickname(this, new ChangedNicknameEventArgs()
            {
                NewNickname = newNickname,
                OldNickname = oldNickname,
            });
        }

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
        /// Node connected event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeConnected(IPAddress ip, string nickname)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Nickname = nickname,
                NodeIP = ip
            });
        }

        /// <summary>
        /// Node disconnected event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeDisconnected(IPAddress ip, string nickname)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                Nickname = nickname
            });
        }

        /// <summary>
        /// Node resumed event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeResumed(IPAddress ip, string nickname)
        {
            NodeResumed(this, new NodeConnectionStatusEventArgs()
            {
                Nickname = nickname
            });
        }

        /// <summary>
        /// Node suspended event.
        /// </summary>
        /// <param name="ip">Node ip</param>
        /// <param name="nickname">Node nickname</param>
        public virtual void OnNodeSuspended(IPAddress ip, string nickname)
        {
            NodeSuspended(this, new NodeConnectionStatusEventArgs()
            {
                Nickname = nickname
            });
        }

        /// <summary>
        /// Received message event.
        /// </summary>
        /// <param name="content">Message content</param>
        /// <param name="nickname">Sender nickname</param>
        /// <param name="target">Message target</param>
        public virtual void OnReceivedMessage(string content, string nickname, MessageTarget target)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                Nickname = nickname,
                Target = target
            });
        }
    }
}