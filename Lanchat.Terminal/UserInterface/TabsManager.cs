using System.Linq;
using Lanchat.Core.Network;
using Lanchat.Terminal.UserInterface.Controls;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public static class TabsManager
    {
        private static readonly Tab FileTransferViewTab;
        private static readonly Tab HomeViewTab;
        private static readonly Tab MainViewTab;

        static TabsManager()
        {
            HomeViewTab = new Tab("Lanchat", HomeView);
            MainViewTab = new Tab("Lanchat", MainChatView);
            FileTransferViewTab = new Tab("File transfer", FileTransfersView);

            Window.TabPanel.SystemTabs.AddTab(HomeViewTab);
            Window.TabPanel.SystemTabs.AddTab(new Tab("Detected users", new DetectedUsersView()));
            Window.TabPanel.SystemTabs.AddTab(FileTransferViewTab);
            Window.TabPanel.SelectTab(HomeViewTab);
        }

        public static HomeView HomeView { get; } = new();
        public static ChatView MainChatView { get; } = new();
        public static FileTransfersView FileTransfersView { get; } = new();

        public static void ShowMainChatView()
        {
            if (Window.TabPanel.SystemTabs.Tabs.First().Content is HomeView)
            {
                Window.TabPanel.SystemTabs.ReplaceTab(HomeViewTab, MainViewTab);
            }
            
            if (Window.TabPanel.CurrentTab.Content is HomeView)
            {
                Window.TabPanel.SelectTab(MainViewTab);
            }
        }

        public static Tab AddPrivateChatView(INode node)
        {
            var chatView = new ChatView(node);
            var chatTab = new Tab(node.User.Nickname, chatView) { Id = node.Id };
            Window.TabPanel.ChatTabs.AddTab(chatTab);
            return chatTab;
        }

        public static void ClosePrivateChatView(INode node)
        {
            var chatTab = Window.TabPanel.AllTabs.First(x => x.Id == node.Id);
            if (Window.TabPanel.CurrentTab == chatTab)
            {
                Window.TabPanel.SelectTab(Window.TabPanel.AllTabs[0]);
            }

            Window.TabPanel.ChatTabs.RemoveTab(chatTab);
        }

        public static void UpdateNickname(INode node)
        {
            var tab = Window.TabPanel.AllTabs.First(x => x.Content is ChatView chatView && chatView.Node == node);
            tab!.Header.UpdateText(node.User.Nickname);
            Window.TabPanel.ChatTabs.RefreshHeaders();
        }

        public static void SignalNewMessage()
        {
            if (Window.TabPanel.CurrentTab != MainViewTab)
            {
                MainViewTab.Header.MarkAsUnread();
            }
        }

        public static void SignalFileTransfer()
        {
            if (Window.TabPanel.CurrentTab != FileTransferViewTab)
            {
                FileTransferViewTab.Header.MarkAsUnread();
            }
        }

        public static void SignalPrivateNewMessage(INode node = null)
        {
            var tab = Window.TabPanel.AllTabs.First(x => x.Content is ChatView chatView && chatView.Node == node);
            if (Window.TabPanel.CurrentTab != tab)
            {
                tab.Header.MarkAsUnread();
            }
        }

        public static DebugView AddDebugView()
        {
            var debugView = new DebugView();
            var debugTab = new Tab("Debug", debugView);
            Window.TabPanel.SystemTabs.AddTab(debugTab);
            return debugView;
        }
    }
}