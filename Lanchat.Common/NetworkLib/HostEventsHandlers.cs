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
                Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
            }
        }

        internal void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
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
                network.CreateNode(new Node(e.Sender.Id, e.Sender.Port, e.SenderIP), false);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void OnReceivedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine("Received handshake");
            Trace.Indent();
            Trace.WriteLine(e.NodeHandshake.Nickname);
            Trace.WriteLine(e.SenderIP);
            Trace.Unindent();

            // If node already crated just accept handhshake
            if (GetNode(e.SenderIP) != null)
            {
                var node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
                Trace.WriteLine("Node found and handshake accepted");
            }

            // If list doesn't contain node with this ip create node and accept handshake
            else
            {
                var node = new Node(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                network.CreateNode(node, false);
                Trace.WriteLine("New node created after recieved handshake");
                node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
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
            var node = GetNode(e.SenderIP);
            node.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        internal void OnReceivedList(object o, ReceivedListEventArgs e)
        {
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
                if (!string.IsNullOrWhiteSpace(content))
                {
                    Trace.WriteLine(node.Nickname + ": " + content);
                    network.Events.OnReceivedMessage(content, node.Nickname);
                }
            }
            else
            {
                Trace.WriteLine($"Message from muted user ({e.SenderIP}) blocked");
            }
        }

        internal void OnReceivedRequest(object o, ReceivedRequestEventArgs e)
        {
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
            if (node != null)
            {
                var nickname = node.ClearNickname;
                Trace.WriteLine(node.Nickname + " disconnected");
                network.Events.OnNodeDisconnected(node.Ip, node.Nickname);
                network.NodeList.Remove(node);
                node.Dispose();
                CheckNickcnameDuplicates(nickname);
            }
            else
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        private Node GetNode(IPAddress ip)
        {
            return network.NodeList.Find(x => x.Ip.Equals(ip));
        }
    }
}