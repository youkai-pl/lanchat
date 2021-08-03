using System;

namespace Lanchat.Terminal.UserInterface
{
    public static class Writer
    {
        public static void WriteText(string text)
        {
            var writeable = Window.TabPanel.CurrentTab.Content as IWriteable;
            writeable?.AddText(text, ConsoleColor.White);
        }

        public static void WriteError(string text)
        {
            var writeable = Window.TabPanel.CurrentTab.Content as IWriteable;
            writeable?.AddText(text, ConsoleColor.Red);
        }

        public static void WriteStatus(string text)
        {
            TabsManager.MainChatView.AddText(text, ConsoleColor.White);
            TabsManager.SignalNewMessage();
        }
    }
}