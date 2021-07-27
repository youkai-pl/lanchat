using System;
using Lanchat.Terminal.UserInterface.Controls;

namespace Lanchat.Terminal.UserInterface
{
    public class Writer
    {
        private readonly TabPanel tabPanel;

        public Writer(TabPanel tabPanel)
        {
            this.tabPanel = tabPanel;
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
    }
}