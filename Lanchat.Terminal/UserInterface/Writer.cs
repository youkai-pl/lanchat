using System;

namespace Lanchat.Terminal.UserInterface
{
    public static class Writer
    {
        public static void WriteText(string text)
        {
            Write(text, ConsoleColor.White);
        }

        public static void WriteError(string text)
        {
            Write(text, ConsoleColor.Red);
        }

        public static void WriteStatus(string text)
        {
            WriteOnMainChat(text, ConsoleColor.White);
        }

        private static void Write(string text, ConsoleColor color)
        {
            if (Window.TabPanel.CurrentTab.Content is IWriteable writeable)
            {
                writeable.AddText(text, color);
            }
            else
            {
                WriteOnMainChat(text, color);
            }
        }

        private static void WriteOnMainChat(string text, ConsoleColor color)
        {
            TabsManager.ShowMainChatView();
            TabsManager.MainChatView.AddText(text, color);
            TabsManager.SignalNewMessage();
        }
    }
}