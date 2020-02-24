using Lanchat.Common.Types;
using System;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib.Handlers
{
    internal class NodeEventsHandlers
    {
        private readonly Network network;
        private readonly Node node;

        public NodeEventsHandlers(Network network, Node node)
        {
            this.network = network;
            this.node = node;

            node.Events.StateChanged += OnStateChanged;
            node.Events.HandshakeAccepted += OnHandshakeAccepted;
            node.ConnectionTimer.Elapsed += OnConnectionTimer;
            node.Events.ReceivedHandshake += OnReceivedHandshake;
            node.Events.ReceivedKey += OnReceivedKey;
            node.Events.RecievedMessage += OnReceivedMessage;
            node.Events.ReceivedList += OnReceivedList;
            node.Events.ReceivedHeartbeat += OnReceivedHeartbeat;
            node.Events.ChangedNickname += OnChangedNickname;
            node.Events.NodeDisconnected += OnNodeDisconnected;
        }

        internal void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            var oldNickname = node.Nickname;

            if (oldNickname != e.NewNickname)
            {
                node.Nickname = e.NewNickname;
                network.CheckNickcnameDuplicates(e.NewNickname);
                network.Events.OnChangedNickname(oldNickname, e.NewNickname);
                Trace.WriteLine($"[NETOWRK] Nickname change ({node.Ip} {oldNickname} / {e.NewNickname})");
            }
        }

        internal void OnNodeDisconnected(object sender, EventArgs e)
        {
            network.CloseNode(node);
        }

        internal void OnReceivedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] Received handshake ({node.Ip} / {e.NodeHandshake.Nickname})");
            node.AcceptHandshake(e.NodeHandshake);
            Trace.WriteLine($"[NETOWRK] Handshake accepted ({node.Ip})");

            network.CheckNickcnameDuplicates(e.NodeHandshake.Nickname);
        }

        internal void OnReceivedHeartbeat(object sender, EventArgs e)
        {
            node.Heartbeat = true;
        }

        internal void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] AES key received ({node.Ip})");
            node.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        internal void OnReceivedList(object o, ReceivedListEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] Nodes list received");
            foreach (var item in e.List)
            {
                var ip = IPAddress.Parse(item.Ip);

                if (!ip.Equals(e.LocalAddress))
                {
                    network.CreateNode(ip, item.Port);
                }
            }
        }

        internal void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            if (!node.Mute)
            {
                var content = node.RemoteAes.Decode(e.Content);
                Trace.WriteLine($"[NETOWRK] Message received ({node.Ip})");
                if (!string.IsNullOrWhiteSpace(content))
                {
                    network.Events.OnReceivedMessage(content, node.Nickname);
                }
            }
            else
            {
                Trace.WriteLine($"[NETOWRK] Message muted ({node.Ip})");
            }
        }

        private void OnHandshakeAccepted(object sender, EventArgs e)
        {
            node.Client.SendHandshake(new Handshake(network.Nickname, network.PublicKey, network.HostPort));
            node.Client.SendList(network.NodeList);
        }

        private void OnConnectionTimer(object o, EventArgs e)
        {
            node.ConnectionTimer.Dispose();

            if (node.State != Status.Ready)
            {
                Trace.WriteLine($"[NODE] Connection timed out ({node.Ip})");
                network.NodeList.Remove(node);
                node.Dispose();
            }
        }

        private void OnStateChanged(object sender, EventArgs e)
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
                // network.Events.OnNodeSuspended(node.Ip, node.Nickname);
                // Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / suspended)");
                network.CloseNode(node);
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