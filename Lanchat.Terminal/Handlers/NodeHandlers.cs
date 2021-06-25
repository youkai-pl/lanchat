using System;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.Handlers
{
    public class NodeHandlers
    {
        private readonly INode node;
        private readonly TabsManager tabsManager;
        private ChatView privateChatView;

        public NodeHandlers(INode node, TabsManager tabsManager)
        {
            this.node = node;
            this.tabsManager = tabsManager;
            
            node.Connected += NodeOnConnected;
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
        }

        private void NodeOnConnected(object sender, EventArgs e)
        {
            privateChatView = tabsManager.CreatePrivateChatView(node);
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            tabsManager.MainChatView.AddMessage(e, node.User.Nickname);
        }
        
        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            privateChatView.AddMessage(e, node.User.Nickname);
        }
    }
}