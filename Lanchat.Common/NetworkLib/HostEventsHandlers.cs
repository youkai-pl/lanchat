using Lanchat.Common.Types;
using System;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    // Event handlers
    internal class HostEventsHandlers
    {
        private readonly Network network;

        // Constructor
        internal HostEventsHandlers(Network network)
        {
            this.network = network;
        }

        // Changed nickname
        internal void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            Node node = GetNode(e.SenderIP);
            var oldNickname = node.Nickname;

            if (oldNickname != e.NewNickname)
            {
                // Change node nickname
                node.Nickname = e.NewNickname;

                // Check is nickname duplicated
                CheckNickcnameDuplicates(e.NewNickname);
                network.Events.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);

                // Emit event
                Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
            }
        }

        // Node connected
        internal void OnNodeConnected(object o, NodeConnectionStatusEventArgs e)
        {
            // If broadcast isn't already received host will create node when the handshake is received
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
        }

        // Node disconnected
        internal void OnNodeDisconnected(object o, NodeConnectionStatusEventArgs e)
        {
            // Find node in list
            var node = GetNode(e.NodeIP);

            // If node exist delete it
            if (node != null)
            {
                CloseNode(node);
            }
            // If node doesn't exist log exception
            else
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        // Received broadcast
        internal void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (IsNodeExist(e.Sender, e.SenderIP))
            {
                // Create new node
                try
                {
                    network.CreateNode(new Node(e.Sender.Id, e.Sender.Port, e.SenderIP), false);
                }
                catch (Exception ex)
                {
                    Trace.Write("Connecting error: " + ex.Message);
                }
            }
        }

        // Recieved handshake
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
                // Create new node
                network.CreateNode(new Node(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP), false);
                Trace.WriteLine("New node created after recieved handshake");

                // Accept handshake
                var node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
            }

            // Add number to peers with same nicknames
            CheckNickcnameDuplicates(e.NodeHandshake.Nickname);
        }

        // Receieved heartbeat
        internal void OnReceivedHeartbeat(object o, ReceivedHeartbeatEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            try
            {
                node.Heartbeat = true;
                // Trace.WriteLine($"({e.SenderIP}): heartbeat received");
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"({e.SenderIP}): cannot handle heartbeat");
                Trace.WriteLine(ex.Message);
            }
        }

        // Receieved symetric key
        internal void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            node.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        // Receieved message
        internal void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var node = GetNode(e.SenderIP);

            if (!node.Mute)
            {
                var content = node.RemoteAes.Decode(e.Content);
                Trace.WriteLine(node.Nickname + ": " + content);
                network.Events.OnReceivedMessage(content, node.Nickname);
            }
            else
            {
                Trace.WriteLine($"Message from muted user ({e.SenderIP}) blocked");
            }
        }

        // Received request
        internal void OnReceivedRequest(object o, ReceivedRequestEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            if (node != null)
            {
                node.Client.SendNickname(network.Nickname);
            }
        }

        // Received list
        internal void OnReceivedList(object o, ReceivedListEventArgs e)
        {
            foreach (var item in e.List)
            {
                try
                {
                    network.CreateNode(new Node(item.Port, IPAddress.Parse(item.Ip)), false);
                }
                catch
                {
                    Trace.WriteLine("Create node by list failed");
                }
            }
        }

        // Get node by IP
        private Node GetNode(IPAddress ip)
        {
            return network.NodeList.Find(x => x.Ip.Equals(ip));
        }

        // Check nickname duplicates
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

        // Close node
        private void CloseNode(Node node)
        {
            var nickname = node.ClearNickname;

            // Log disconnect
            Trace.WriteLine(node.Nickname + " disconnected");

            // Emit event
            network.Events.OnNodeDisconnected(node.Ip, node.Nickname);

            // Remove node from list
            network.NodeList.Remove(node);

            // Dispose node
            node.Dispose();

            // Delete the number if nicknames are not duplicated now
            CheckNickcnameDuplicates(nickname);
        }

        // Check is paperplane come from self or user alredy exist in list
        private bool IsNodeExist(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }
    }
}