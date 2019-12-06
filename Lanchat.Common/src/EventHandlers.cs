using System.Diagnostics;

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
            if (network.IsCanAdd(e.Sender, e.SenderIP))
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
                network.OnNodeDisconnected(node.Ip, node.Nickname);
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
                network.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).AcceptHandshake(e.NodeHandshake);
            }
            else
            {
                // Create new node
                Trace.WriteLine("New node created after recieved handshake");
                network.CreateNode(e.NodeHandshake.Id, e.NodeHandshake.Port, e.SenderIP);
                network.OnNodeConnected(e.SenderIP, e.NodeHandshake.Nickname);
                network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).AcceptHandshake(e.NodeHandshake);
            }
        }

        // Recieved message
        public void OnRecievedMessage(object o, RecievedMessageEventArgs e)
        {
            var userNickname = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP)).Nickname;
            Trace.WriteLine(userNickname + ": " + e.Content);
            network.OnRecievedMessage(e.Content, userNickname);
        }

        // Changed nickname
        public void OnChangedNickname(object o, ChangedNicknameEventArgs e)
        {
            var user = network.NodeList.Find(x => x.Ip.Equals(e.SenderIP));
            var oldNickname = user.Nickname;
            user.Nickname = e.NewNickname;
            network.OnChangedNickname(oldNickname, e.NewNickname, e.SenderIP);
            Trace.WriteLine($"{oldNickname} nickname changed to {e.NewNickname}");
        }
    }
}