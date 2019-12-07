using Lanchat.Common.HostLib;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    // Event handlers
    public class EventHandlers
    {
        private readonly Network network;

        public EventHandlers(Network network)
        {
            this.network = network;
        }

        // Recieved broadcast
        public void OnRecievedBroadcast(object o, RecievedBroadcastEventArgs e)
        {
            if (IsCanAdd(e.Sender, e.SenderIP))
            {
                // Create new node
                network.CreateNode(e.Sender.Id, e.Sender.Port, e.SenderIP);
            }
        }

        // Node connected
        public void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
        }

        // Node disconnected
        public void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            try
            {
                // Remove node from list
                var node = network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
                Trace.WriteLine(node.Nickname + " disconnected");
                network.Events.OnNodeDisconnected(node.Ip, node.Nickname);
                network.NodeList.RemoveAll(x => x.Ip.Equals(e.NodeIP));
            }
            catch
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        // Recieved handshake
        public void OnRecievedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine("Recieved handshake");
            Trace.Indent();
            Trace.WriteLine(e.NodeHandshake.Nickname);
            Trace.WriteLine(e.SenderIP);
            Trace.Unindent();

            if (network.NodeList.Exists(x => x.Ip.Equals(e.SenderIP)))
            {
                Trace.WriteLine("Node found and handshake accepted");
                network.Events.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
                user.Connection.SendKey(user.PublicKey, "test");
            }
            else
            {
                // Create new node
                Trace.WriteLine("New node created after recieved handshake");
                network.CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                network.Events.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
                user.Connection.SendKey(user.PublicKey, "test");
            }
        }

        // Recieved symetric key
        public void OnRecievedKey(object o, RecievedKeyEventArgs e)
        {
            Trace.WriteLine(network.Cryptography.AsymetricDecode(e.Key));
        }

        // Recieved message
        public void OnRecievedMessage(object o, RecievedMessageEventArgs e)
        {
            var userNickname = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).Nickname;
            Trace.WriteLine(userNickname + ": " + e.Content);
            network.Events.OnRecievedMessage(e.Content, userNickname);
        }

        // Changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            var oldNickname = user.Nickname;
            user.Nickname = e.NewNickname;
            network.Events.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);
            Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
        }

        // Check is paperplane come from self or user alredy exist in list
        public bool IsCanAdd(Paperplane broadcast, IPAddress senderIp)
        {
            return broadcast.Id != network.Id && !network.NodeList.Exists(x => x.Id.Equals(broadcast.Id)) && !network.NodeList.Exists(x => x.Ip.Equals(senderIp));
        }
    }
}