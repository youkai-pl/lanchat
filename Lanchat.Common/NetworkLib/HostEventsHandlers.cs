﻿using Lanchat.Common.Types;
using System;
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
            var node = GetNode(e.NodeIP);


            CloseNode(node);

        }

        internal void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (IsNodeExist(e.Sender, e.SenderIP))
            {
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
                network.CreateNode(new Node(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP), false);
                Trace.WriteLine("New node created after recieved handshake");
                var node = GetNode(e.SenderIP);
                node.AcceptHandshake(e.NodeHandshake);
            }

            CheckNickcnameDuplicates(e.NodeHandshake.Nickname);
        }

        internal void OnReceivedHeartbeat(object o, ReceivedHeartbeatEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            try
            {
                node.Heartbeat = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"({e.SenderIP}): cannot handle heartbeat");
                Trace.WriteLine(ex.Message);
            }
        }

        internal void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            node.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

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

        internal void OnReceivedRequest(object o, ReceivedRequestEventArgs e)
        {
            var node = GetNode(e.SenderIP);
            if (node != null)
            {
                node.Client.SendNickname(network.Nickname);
            }
        }

        // Methods

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

        // SHIT
        private bool IsNodeExist(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }
    }
}