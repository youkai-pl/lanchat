using Lanchat.Common.Types;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    internal class HostEventsHandlers
    {
        private readonly Network network;

        internal HostEventsHandlers(Network network)
        {
            this.network = network;
        }

        // Handlers

        internal void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            Node node = GetNode(e.SenderIP);
            var oldNickname = node.Nickname;

            if (oldNickname != e.NewNickname)
            {
                node.Nickname = e.NewNickname;
                CheckNickcnameDuplicates(e.NewNickname);
                network.Events.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);
                Trace.WriteLine($"[NETOWRK] Nickname change ({node.Ip} {oldNickname} / {e.NewNickname})");
            }
        }

        internal void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
        }

        internal void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            CloseNode(GetNode(e.NodeIP));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (CheckBroadcastID(e.Sender, e.SenderIP))
            {
                Trace.WriteLine($"[NETOWRK] Broadcast received ({e.SenderIP})");
                network.CreateNode(new Node(e.Sender.Id, e.Sender.Port, e.SenderIP), false);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void OnReceivedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] Received handshake ({e.SenderIP} / {e.NodeHandshake.Nickname})");

            // If node already crated just accept handhshake
            if (GetNode(e.SenderIP) != null)
            {
                var node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
                Trace.WriteLine($"[NETOWRK] Handshake recieved for existing node ({e.SenderIP})");
            }

            // If list doesn't contain node with this ip create node and accept handshake
            else
            {
                var node = new Node(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                network.CreateNode(node, false);
                node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
                Trace.WriteLine($"[NETOWRK] Handshake recieved for new node ({e.SenderIP})");
            }

            CheckNickcnameDuplicates(e.NodeHandshake.Nickname);
        }

        internal void OnReceivedHeartbeat(object o, ReceivedHeartbeatEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            node.Heartbeat = true;
        }

        internal void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] AES key received ({e.SenderIP})");
            var node = GetNode(e.SenderIP);
            node.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void OnReceivedList(object o, ReceivedListEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] Nodes list received");
            foreach (var item in e.List)
            {
                network.CreateNode(new Node(item.Port, IPAddress.Parse(item.Ip)), false);
            }
        }

        internal void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var node = GetNode(e.SenderIP);

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
                Trace.WriteLine($"[NETOWRK] Message muted ({e.SenderIP})");
            }
        }

        internal void OnReceivedRequest(object o, ReceivedRequestEventArgs e)
        {
            Trace.WriteLine($"[NETOWRK] Request received ({e.SenderIP} / {e.Type})");
            var node = GetNode(e.SenderIP);
            if (node != null)
            {
                node.Client.SendNickname(network.Nickname);
            }
        }

        // Methods

        private bool CheckBroadcastID(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }

        private void CheckNickcnameDuplicates(string nickname)
        {
            var nodes = network.NodeList.FindAll(x => x.ClearNickname == nickname);
            if (nodes.Count > 1)
            {
                var index = 1;
                foreach (var item in nodes)
                {
                    item.NicknameNum = index;
                    index++;
                }
            }
            else if (nodes.Count > 0)
            {
                nodes[0].NicknameNum = 0;
            }
        }

        private void CloseNode(Node node)
        {
            var nickname = node.ClearNickname;
            Trace.WriteLine($"[NETWORK] Node disconnected ({node.Ip})");
            network.Events.OnNodeDisconnected(node.Ip, node.Nickname);
            network.NodeList.Remove(node);
            node.Dispose();
            CheckNickcnameDuplicates(nickname);
        }

        private Node GetNode(IPAddress ip)
        {
            return network.NodeList.Find(x => x.Ip.Equals(ip));
        }
    }
}