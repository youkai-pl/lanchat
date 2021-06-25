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
            MainChatView = new ChatView("#main", true);
            tabPanel.AddChatTab(MainChatView);
            tabPanel.AddSystemTab(new Tab("Detected users", new DetectedUsersView()));
            tabPanel.AddSystemTab(new Tab("File transfer", new FileTransfersView()));
        }
        
        public ChatView CreatePrivateChatView(INode node)
        {
            var chatView = new ChatView(node.User.Nickname, false, node);
            tabPanel.AddChatTab(chatView);
            return chatView;
        }
    }
}