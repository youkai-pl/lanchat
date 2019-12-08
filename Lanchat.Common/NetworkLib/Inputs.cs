using Lanchat.Common.HostLib.Types;
using System.Diagnostics;
using System.Net;

namespace Lanchat.Common.NetworkLib
{
    // Event handlers
    public class Inputs
    {
        // Constructor
        public Inputs(Network network)
        {
            this.network = network;
        }

        private readonly Network network;

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
            // Find node in list
            var node = network.NodeList.Find(x => x.Ip.Equals(e.NodeIP));
            // If node exist delete it
            if (node != null)
            {
                // Log disconnect
                Trace.WriteLine(node.Nickname + " disconnected");
                // Remove node from list
                network.NodeList.Remove(node);
                // Emit event
                network.Events.OnNodeDisconnected(node.Ip, node.Nickname);
            }
            // If node doesn't exist log exception
            else
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

            // If node already crated just accept handhshake
            if (network.NodeList.Exists(x => x.Ip.Equals(e.SenderIP)))
            {
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
                network.Events.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                Trace.WriteLine("Node found and handshake accepted");
            }
            // If list doesn't contain node with this ip create node and accept handshake
            else
            {
                // Create new node
                network.CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                network.Events.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                Trace.WriteLine("New node created after recieved handshake");

                // Accept handshake
                var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
                user.AcceptHandshake(e.NodeHandshake);
            }
        }

        // Receieved symetric key
        public void OnReceivedKey(object o, RecievedKeyEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            user.CreateRemoteAes(network.Rsa.Decode(e.AesKey), network.Rsa.Decode(e.AesIV));
        }

        // Recieved message
        public void OnReceivedMessage(object o, ReceivedMessageEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            var content = user.RemoteAes.Decode(e.Content);
            Trace.WriteLine(user.Nickname + ": " + content);
            network.Events.OnReceivedMessage(content, user.Nickname);
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