using Lanchat.Common.NetworkLib.EventsArgs;
using Lanchat.Common.Types;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib.Host
{
    internal class HostEventsHandlers
    {
        private readonly Network network;

        internal HostEventsHandlers(Network network, HostInstance host)
        {
            this.network = network;
            host.Events.NodeConnected += OnNodeConnected;
            host.Events.RecievedBroadcast += OnReceivedBroadcast;
        }

        internal void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            Trace.WriteLine($"[NETWORK] Connection detected ({e.Ip})");

            var node = network.Methods.GetNode(e.Ip);

            if (node != null)
            {
                node.Socket = e.Socket;
                node.StartProcess();
                Trace.WriteLine($"[NETWORK] Node found. Socket assigned ({e.Ip})");
            }
            else
            {
                network.CreateNode(socket: e.Socket);
            }
        }

        internal void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (CheckBroadcastID(e.Sender, e.SenderIP))
            {
                Trace.WriteLine($"[NETOWRK] Broadcast received ({e.SenderIP})");
                network.CreateNode(e.SenderIP, e.Sender.Port);
            }
        }

        // Methods

        private bool CheckBroadcastID(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }
    }
}