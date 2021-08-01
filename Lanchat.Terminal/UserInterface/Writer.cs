using System;
using ConsoleGUI.Data;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public class Writer
    {
        private readonly TabPanel tabPanel;
        private readonly TabsManager tabsManager;

        public Writer(TabPanel tabPanel, TabsManager tabsManager)
        {
            this.tabPanel = tabPanel;
            this.tabsManager = tabsManager;
        }

        public void WriteText(string text)
        {
            var writeable = tabPanel.CurrentTab.Content as IWriteable;
            writeable?.AddText(text, ConsoleColor.White);
        }

        public void WriteError(string text)
        {
            var writeable = tabPanel.CurrentTab.Content as IWriteable;
            writeable?.AddText(text, ConsoleColor.Red);
        }

        public void WriteStatus(string text)
        {
            tabsManager.MainChatView.AddText(text, ConsoleColor.White);
            tabsManager.SignalNewMessage();
        }
    }
}