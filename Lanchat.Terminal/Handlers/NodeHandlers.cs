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
        private readonly TabsManager tabsManager;
        private Tab privateChatTab;
        private ChatView privateChatView;

        public NodeHandlers(INode node, TabsManager tabsManager)
        {
            this.node = node;
            this.tabsManager = tabsManager;

            node.Connected += NodeOnConnected;
            node.Disconnected += NodeOnDisconnected;
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
            node.User.NicknameUpdated += UserOnNicknameUpdated;
        }

        private void NodeOnConnected(object sender, EventArgs e)
        {
            tabsManager.ShowMainChatView();
            privateChatTab = tabsManager.AddPrivateChatView(node);
            privateChatView = (ChatView)privateChatTab.Content;
            Window.Writer.WriteStatus(string.Format(Resources._Connected, node.User.Nickname));
        }

        private void NodeOnDisconnected(object sender, EventArgs e)
        {
            tabsManager.ClosePrivateChatView(node);
            Window.Writer.WriteStatus(string.Format(Resources._Disconnected, node.User.Nickname));
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            tabsManager.MainChatView.AddMessage(e, node.User.Nickname);
            tabsManager.SignalNewMessage();
        }

        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            privateChatView.AddMessage(e, node.User.Nickname);
            tabsManager.SignalPrivateNewMessage(node);
        }

        private void UserOnNicknameUpdated(object sender, string e)
        {
            tabsManager.UpdateNickname(node);
            Window.Writer.WriteStatus(string.Format(Resources._NicknameChanged, node.User.PreviousNickname,
                node.User.Nickname));
        }
    }
}