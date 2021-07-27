using System.Linq;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public class TabsManager
    {
        private readonly Tab homeViewTab;
        private readonly Tab mainViewTab;
        private readonly TabPanel tabPanel;

        public TabsManager(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            HomeView = new HomeView();
            MainChatView = new ChatView();
            FileTransfersView = new FileTransfersView();
            
            homeViewTab = new Tab("Lanchat", HomeView);
            mainViewTab = new Tab("Lanchat", MainChatView);
            tabPanel.SystemTabs.AddTab(homeViewTab);
            tabPanel.SystemTabs.AddTab(new Tab("Detected users", new DetectedUsersView()));
            tabPanel.SystemTabs.AddTab(new Tab("File transfer", FileTransfersView));
            tabPanel.SelectTab(homeViewTab);

        }

        public HomeView HomeView { get; }
        public ChatView MainChatView { get; }
        public FileTransfersView FileTransfersView { get; }

        public void ShowMainChatView()
        {
            if (tabPanel.SystemTabs.Tabs.First().Content is HomeView)
            {
                tabPanel.SystemTabs.ReplaceTab(homeViewTab, mainViewTab);
            }
        }

        public ChatView AddPrivateChatView(INode node)
        {
            var chatView = new ChatView(node);
            var chatTab = new Tab(node.User.Nickname, chatView) {Id = node.Id};
            tabPanel.ChatTabs.AddTab(chatTab);
            return chatView;
        }
        
        public void ClosePrivateChatView(INode node)
        {
            var chatTab = tabPanel.AllTabs.FirstOrDefault(x => x.Id == node.Id);
            tabPanel.ChatTabs.RemoveTab(chatTab);
        }
        
        public void UpdateNickname(INode node)
        {
            var tab = tabPanel.AllTabs.FirstOrDefault(x => x.Content is ChatView chatView && chatView.Node == node);
            tab!.Header.UpdateText(node.User.Nickname);
            tabPanel.ChatTabs.RefreshHeaders();
        }
        
        public DebugView AddDebugView()
        {
            var debugView = new DebugView();
            var debugTab = new Tab("Debug", debugView);
            tabPanel.SystemTabs.AddTab(debugTab);
            return debugView;
        }
    }
}