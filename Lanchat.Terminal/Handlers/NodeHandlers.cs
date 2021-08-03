using System;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Handlers
{
    public class NodeHandlers
    {
        private readonly INode node;
        private Tab privateChatTab;
        private ChatView privateChatView;

        public NodeHandlers(INode node)
        {
            this.node = node;

            node.Connected += NodeOnConnected;
            node.Disconnected += NodeOnDisconnected;
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
            node.User.NicknameUpdated += UserOnNicknameUpdated;
        }

        private void NodeOnConnected(object sender, EventArgs e)
        {
            TabsManager.ShowMainChatView();
            privateChatTab = TabsManager.AddPrivateChatView(node);
            privateChatView = (ChatView)privateChatTab.Content;
            Writer.WriteStatus(string.Format(Resources._Connected, node.User.Nickname));
        }

        private void NodeOnDisconnected(object sender, EventArgs e)
        {
            TabsManager.ClosePrivateChatView(node);
            Writer.WriteStatus(string.Format(Resources._Disconnected, node.User.Nickname));
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            TabsManager.MainChatView.AddMessage(e, node.User.Nickname);
            TabsManager.SignalNewMessage();
        }

        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            privateChatView.AddMessage(e, node.User.Nickname);
            TabsManager.SignalPrivateNewMessage(node);
        }

        private void UserOnNicknameUpdated(object sender, string e)
        {
            TabsManager.UpdateNickname(node);
            Writer.WriteStatus(
                string.Format(Resources._NicknameChanged, node.User.PreviousNickname, node.User.Nickname));
        }
    }
}