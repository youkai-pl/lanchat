using System;
using Lanchat.Core.Identity;
using Lanchat.Core.Network;

namespace Lanchat.Ipc
{
    public class NodeHandlers
    {
        private readonly INode node;

        public NodeHandlers(INode node)
        {
            this.node = node;

            node.Connected += NodeOnConnected;
            node.Disconnected += NodeOnDisconnected;
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
            node.User.NicknameUpdated += UserOnNicknameUpdated;
            node.User.StatusUpdated += UserOnStatusUpdated;
        }

        private void NodeOnConnected(object sender, EventArgs e)
        {
            Program.IpcSocket.Send($"connected/{node.Id}");
        }

        private void NodeOnDisconnected(object sender, EventArgs e)
        {
            Program.IpcSocket.Send($"disconnected/{node.Id}");
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            Program.IpcSocket.Send($"{node.Id}/message/{e}");
        }

        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            Program.IpcSocket.Send($"{node.Id}/private_message/{e}");
        }

        private void UserOnNicknameUpdated(object sender, string e)
        {
            Program.IpcSocket.Send($"{node.Id}/nickname_update/{e}");
        }

        private void UserOnStatusUpdated(object sender, UserStatus e)
        {
            Program.IpcSocket.Send($"{node.Id}/status_update/{e.ToString().ToLower()}");
        }
    }
}