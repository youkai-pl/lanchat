using System.Collections.Generic;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.UserInterface
{
    public class TabsManager
    {
        public ChatView MainChatView { get; }
        public List<ChatView> ChatViews { get; } = new();

        private readonly TabPanel tabPanel;

        public TabsManager(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            MainChatView = new ChatView("main", true);
            ChatViews.Add(MainChatView);
            tabPanel.AddTab(MainChatView);
        }
        
        public ChatView CreatePrivateChatView(INode node)
        {
            var chatView = new ChatView(node.User.Nickname, false, node);
            ChatViews.Add(chatView);
            tabPanel.AddTab(chatView);
            return chatView;
        }
    }
}