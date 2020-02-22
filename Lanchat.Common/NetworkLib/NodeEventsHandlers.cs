using Lanchat.Common.Types;
using System;
using System.Diagnostics;

namespace Lanchat.Common.NetworkLib
{
    internal class NodeEventsHandlers
    {
        private readonly Network network;
        private readonly Node node;

        public NodeEventsHandlers(Network _network, Node _node)
        {
            network = _network;
            node = _node;

            node.StateChanged += OnStatusChanged;
            node.HandshakeAccepted += OnHandshakeAccepted;
            node.HandshakeTimer.Elapsed += OnHandshakeTimeout;
        }

        private void OnHandshakeAccepted(object sender, EventArgs e)
        {
            node.Client.SendHandshake(new Handshake(network.Nickname, network.PublicKey, network.HostPort));
            node.Client.SendList(network.NodeList);
        }

        private void OnHandshakeTimeout(object o, EventArgs e)
        {
            node.HandshakeTimer.Dispose();

            if (node.Handshake == null)
            {
                Trace.WriteLine($"[NODE] Handshake timed out {node.Ip}");
                network.NodeList.Remove(node);
                node.Dispose();
            }
            else
            {
                Trace.WriteLine($"[NODE] Handshake received on time {node.Ip}");
            }
        }

        private void OnStatusChanged(object sender, EventArgs e)
        {
            // Node ready
            if (node.State == Status.Ready)
            {
                network.Events.OnNodeConnected(node.Ip, node.Nickname);
                Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / ready)");
            }

            // Node suspended
            else if (node.State == Status.Suspended)
            {
                network.Events.OnNodeSuspended(node.Ip, node.Nickname);
                Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / suspended)");
            }

            // Node resumed
            else if (node.State == Status.Resumed)
            {
                node.Client.ResumeConnection(network.Nickname);
                node.State = Status.Ready;
                network.Events.OnNodeResumed(node.Ip, node.Nickname);
                Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / resumed)");
            }
        }
    }
}