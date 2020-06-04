using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using System;

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
        /// Node suspended event.
        /// </summary>
        /// <param name="node">Node</param>
        public virtual void OnNodeConnected(NodeInstance node)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Node = node
            });
        }

        /// <summary>
        /// Node suspended event.
        /// </summary>
        /// <param name="node">Node</param>
        public virtual void OnNodeDisconnected(NodeInstance node)
        {
            NodeDisconnected(this, new NodeConnectionStatusEventArgs()
            {
                Node = node
            });
        }

        /// <summary>
        /// Received message event.
        /// </summary>
        /// <param name="content">Message content</param>
        /// <param name="node">Sender</param>
        /// <param name="target">Message target</param>
        public virtual void OnReceivedMessage(string content, NodeInstance node, MessageTarget target)
        {
            ReceivedMessage(this, new ReceivedMessageEventArgs()
            {
                Content = content,
                Node = node,
                Target = target
            });
        }
    }
}