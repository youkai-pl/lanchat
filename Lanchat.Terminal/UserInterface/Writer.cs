using ConsoleGUI.Data;

namespace Lanchat.Terminal.UserInterface
{
    public static class Writer
    {
        public static void WriteText(string text)
        {
            Write(text, Theme.Foreground);
        }

        public static void WriteWarning(string text)
        {
            Write(text, Theme.LogWarning);
        }

        public static void WriteError(string text)
        {
            Write(text, Theme.LogError);
        }

        public static void WriteStatus(string text)
        {
            WriteOnMainChat(text, Theme.LogStatus);
        }

        private static void Write(string text, Color color)
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

        private static void WriteOnMainChat(string text, Color color)
        {
            TabsManager.ShowMainChatView();
            TabsManager.MainChatView.AddText(text, color);
            TabsManager.SignalNewMessage();
        }
    }
}