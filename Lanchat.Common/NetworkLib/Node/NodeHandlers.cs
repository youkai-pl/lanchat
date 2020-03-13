using Lanchat.Common.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib.Node
{
    internal class NodeHandlers
    {
        private readonly Network network;
        private readonly NodeInstance node;

        public NodeHandlers(Network network, NodeInstance node)
        {
            this.network = network;
            this.node = node;
        }

        internal void OnChangedNickname(string newNickname)
        {
            var oldNickname = node.Nickname;

            if (oldNickname != newNickname)
            {
                node.Nickname = newNickname;
                network.CheckNickcnameDuplicates(newNickname);
                network.Events.OnChangedNickname(oldNickname, newNickname);
                Trace.WriteLine($"[NETOWRK] Nickname change ({node.Ip} {oldNickname} / {newNickname})");
            }
        }

        internal void OnNodeDisconnected()
        {
            network.CloseNode(node);
        }

        internal void OnReceivedHandshake(Handshake handshake)
        {
            Trace.WriteLine($"[NETOWRK] Received handshake ({node.Ip} / {handshake.Nickname})");
            node.AcceptHandshake(handshake);
            Trace.WriteLine($"[NETOWRK] Handshake accepted ({node.Ip})");

            network.CheckNickcnameDuplicates(handshake.Nickname);
        }

        internal void OnReceivedHeartbeat()
        {
            node.Heartbeat = true;
        }

        internal void OnReceivedKey(Key key)
        {
            Trace.WriteLine($"[NETOWRK] AES key received ({node.Ip})");
            node.CreateRemoteAes(network.Rsa.Decode(key.AesKey), network.Rsa.Decode(key.AesIV));
        }

        internal void OnReceivedList(List<ListItem> list, IPAddress localAddress)
        {
            Trace.WriteLine($"[NETOWRK] Nodes list received");
            foreach (var item in list)
            {
                var ip = IPAddress.Parse(item.Ip);

                if (!ip.Equals(localAddress))
                {
                    network.CreateNode(ip, item.Port);
                }
            }
        }

        internal void OnReceivedMessage(string content, MessageTarget target)
        {
            if (!node.Mute)
            {
                var decodedContent = node.RemoteAes.Decode(content);
                Trace.WriteLine($"[NETOWRK] Message received ({node.Ip})");
                if (!string.IsNullOrWhiteSpace(decodedContent))
                {
                    network.Events.OnReceivedMessage(decodedContent, node, target);
                }
            }
            else
            {
                Trace.WriteLine($"[NETOWRK] Message muted ({node.Ip})");
            }
        }

        internal void OnHandshakeAccepted()
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

        internal void OnStateChanged()
        {
            // Node ready
            if (node.State == Status.Ready)
            {
                network.Events.OnNodeConnected(node);
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
                network.Events.OnNodeResumed(node);
                Trace.WriteLine($"[NETWORK] Node state changed ({node.Ip} / resumed)");
            }
        }
    }
}