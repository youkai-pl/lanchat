using Lanchat.Core.Network;

namespace Lanchat.Ipc
{
    public class NodeHandlers
    {
        private readonly INode node;
        private readonly IpcSocket ipcSocket;

        public NodeHandlers(INode node, IpcSocket socketListener)
        {
            this.node = node;
            this.ipcSocket = socketListener;

            node.Messaging.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, string message)
        {
            ipcSocket.Send(message);
        }
    }
}