using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.NetworkLib.Node;
using Lanchat.Common.Types;
using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib.Host
{
    internal class HostEvents
    {
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        internal event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        internal virtual void OnNodeConnected(NodeInstance node)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Node = node
            });
        }

        internal virtual void OnNodeConnected(Socket socket, IPAddress ip)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Socket = socket,
                Ip = ip
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