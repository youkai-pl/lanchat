using System.Diagnostics;

namespace Lanchat.Common.NetworkLib
{
    public partial class Network
    {
        internal void CloseNode(Node node)
        {
            var nickname = node.ClearNickname;

            // Log disconnect
            Trace.WriteLine(node.Nickname + " disconnected");

            // Emit event
            Events.OnNodeDisconnected(node.Ip, node.Nickname);

            // Remove node from list
            NodeList.Remove(node);

            // Dispose node
            node.Dispose();

            // Delete the number if nicknames are not duplicated now
            CheckNickcnameDuplicates(nickname);
        }
    }
}
