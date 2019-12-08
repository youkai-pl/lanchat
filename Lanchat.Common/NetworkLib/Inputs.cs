using Lanchat.Common.HostLib;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    // Event handlers
    public class Inputs
    {
        private readonly Network network;

        public Inputs(Network network)
        {
            this.network = network;
        }

        // Received broadcast
        public void OnReceivedBroadcast(object o, RecievedBroadcastEventArgs e)
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
            // If broadcast isn't already received host will create node when the handshake is received
            Trace.WriteLine("New connection from: " + e.NodeIP.ToString());
        }

        // Node disconnected
        public void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            try
            {
                // Find node
                var node = network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
                // Remove node from list
                network.NodeList.RemoveAll(x => x.Ip.Equals(e.NodeIP));
                // Emit event
                network.Events.OnNodeDisconnected(node.Ip, node.Nickname);

                Trace.WriteLine(node.Nickname + " disconnected");
            }
            catch
            {
                Trace.WriteLine("Node does not exist");
            }
        }

        // Recieved handshake
        public void OnReceivedHandshake(object o, RecievedHandshakeEventArgs e)
        {
            Trace.WriteLine("Received handshake");
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
        public void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            Trace.WriteLine(network.Cryptography.AsymetricDecode(e.Key));
        }

        // Recieved message
        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var userNickname = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).Nickname;
            Trace.WriteLine(userNickname + ": " + e.Content);
            network.Events.OnReceivedMessage(e.Content, userNickname);
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