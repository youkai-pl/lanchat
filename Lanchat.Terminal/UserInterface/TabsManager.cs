using System.Collections.Generic;
using System.Linq;

namespace Lanchat.Terminal.UserInterface
{
    public class TabsManager
    {
        public List<ChatView> ChatViews { get; } = new();
        
        private readonly TabPanel tabPanel;

        public TabsManager(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
            AddChatView("main");
        }

        public ChatView AddChatView(string name)
        {
            var chatView = ChatViews.FirstOrDefault(x => x.Id == name);
            if (chatView != null)
            {
                return chatView;
            }
            
            chatView = new ChatView(name);
            ChatViews.Add(chatView);
            tabPanel.AddTab(name, chatView);
            
            return chatView;
        }

        public ChatView GetChatView(string name)
        {
            return ChatViews.FirstOrDefault(x => x.Id == name);
        }
    }
}