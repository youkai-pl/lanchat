using Lanchat.Common.Types;
using System;
using System.Net;
using System.Net.Sockets;

namespace Lanchat.Common.NetworkLib
{
    internal class HostEvents
    {
        internal event EventHandler<NodeConnectionStatusEventArgs> NodeConnected;

        internal event EventHandler<RecievedBroadcastEventArgs> RecievedBroadcast;

        internal virtual void OnNodeConnected(Socket socket, IPAddress ip)
        {
            NodeConnected(this, new NodeConnectionStatusEventArgs()
            {
                Socket = socket,
                NodeIP = ip
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