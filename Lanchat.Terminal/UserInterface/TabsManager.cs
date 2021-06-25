using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public class TabsManager
    {
        private readonly TabPanel tabPanel;
        public ChatView MainChatView { get; }

        public TabsManager(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            MainChatView = new ChatView(true);
            tabPanel.AddSystemTab(new Tab("Detected users", new DetectedUsersView()));
            tabPanel.AddSystemTab(new Tab("File transfer", new FileTransfersView()));
            tabPanel.AddChatTab(new Tab("main", MainChatView));
        }
        
        public ChatView CreatePrivateChatView(INode node)
        {
            var chatView = new ChatView(false, node);
            tabPanel.AddChatTab(new Tab(node.User.Nickname, chatView));
            return chatView;
        }
    }
}