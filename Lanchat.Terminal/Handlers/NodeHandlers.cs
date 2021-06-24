using System;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Handlers
{
    public class NodeHandlers
    {
        private readonly INode node;
        private readonly TabsManager tabsManager;
        private readonly ChatView mainChatView;

        public NodeHandlers(INode node, TabsManager tabsManager)
        {
            this.node = node;
            this.tabsManager = tabsManager;
            mainChatView = tabsManager.GetChatView("main");
            
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            mainChatView.AddMessage(e, node.User.Nickname);
        }
    }
}