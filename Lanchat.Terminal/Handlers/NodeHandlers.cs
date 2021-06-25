using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface;

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
            
            node.Messaging.MessageReceived += MessagingOnMessageReceived;
            node.Messaging.PrivateMessageReceived += MessagingOnPrivateMessageReceived;
        }

        private void MessagingOnMessageReceived(object sender, string e)
        {
            tabsManager.MainChatView.AddMessage(e, node.User.Nickname);
        }
        
        private void MessagingOnPrivateMessageReceived(object sender, string e)
        {
            privateChatView ??= tabsManager.GetOrCreatePrivateChatView(node);
            privateChatView.AddMessage(e, node.User.Nickname);
        }
    }
}