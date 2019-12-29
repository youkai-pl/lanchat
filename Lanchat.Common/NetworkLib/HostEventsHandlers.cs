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
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            var oldNickname = user.Nickname;
            user.Nickname = e.NewNickname;

            // Check is nickname duplicated
            CheckNickcnameDuplicates(e.NewNickname);
            network.Events.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);

            // Emit event
            Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
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
            var node = network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));

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
                    CreateNode(e.Sender.Id, e.Sender.Port, e.SenderIP);
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
            if (network.NodeList.Exists(x => x.Ip.Equals(e.SenderIP)))
            {
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
                Trace.WriteLine("Node found and handshake accepted");
            }

            // If list doesn't contain node with this ip create node and accept handshake
            else
            {
                // Create new node
                CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                Trace.WriteLine("New node created after recieved handshake");

                // Accept handshake
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
            }

            // Add number to peers with same nicknames
            CheckNickcnameDuplicates(e.NodeHandshake.Nickname);
        }

        // Receieved heartbeat
        internal void OnReceivedHeartbeat(object o, ReceivedHeartbeatEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            try
            {
                user.Heartbeat = true;
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
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            user.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        // Recieved message
        internal void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));

            if (!user.Mute)
            {
                var content = user.RemoteAes.Decode(e.Content);
                Trace.WriteLine(user.Nickname + ": " + content);
                network.Events.OnReceivedMessage(content, user.Nickname);
            }
            else
            {
                Trace.WriteLine($"Message from muted user ({e.SenderIP}) blocked");
            }
        }

        // Check nickname duplicates
        private void CheckNickcnameDuplicates(string nickname)
        {
            var users = network.NodeList.FindAll(x => x.ClearNickname == nickname);
            if (users.Count > 1)
            {
                var index = 1;
                foreach (var item in users)
                {
                    item.NicknameNum = index;
                    index++;
                }
            }
            else if (users.Count > 0)
            {
                users[0].NicknameNum = 0;
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

        // Create node
        private void CreateNode(Guid id, int port, IPAddress ip)
        {
            // Create new node with parameters
            var node = new Node(id, port, ip);

            // Create node events handlers
            node.ReadyChanged += OnStatusChanged;

            // Add node to list
            network.NodeList.Add(node);

            // Create connection with node
            node.CreateConnection();

            // Send handshake to node
            node.Client.SendHandshake(new Handshake(network.Nickname, network.PublicKey, network.Id, network.HostPort));

            // Log
            Trace.WriteLine("New node created");
            Trace.Indent();
            Trace.WriteLine(node.Ip);
            Trace.WriteLine(node.Port.ToString());
            Trace.Unindent();

            // Ready change event
            void OnStatusChanged(object sender, EventArgs e)
            {
                // Node ready
                if (node.State == Status.Ready)
                {
                    Trace.WriteLine($"({node.Ip}) ready");
                    network.Events.OnNodeConnected(node.Ip, node.Nickname);
                }

                // Node suspended
                else if (node.State == Status.Suspended)
                {
                    Trace.WriteLine($"({node.Ip}) suspended");
                    network.Events.OnNodeSuspended(node.Ip, node.Nickname);
                }

                // Node resumed
                else if (node.State == Status.Resumed)
                {
                    Trace.WriteLine($"({node.Ip}) resumed");
                    node.Client.ResumeConnection();
                    node.State = Status.Ready;
                    network.Events.OnNodeResumed(node.Ip, node.Nickname);
                }
            }
        }

        // Check is paperplane come from self or user alredy exist in list
        private bool IsNodeExist(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }
    }
}