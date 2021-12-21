using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Space;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class TabHeader : SimpleControl
    {
        private readonly Background headerBackground;
        private readonly TextBlock textBlock;

        public TabHeader(string text)
        {
            textBlock = new TextBlock { Text = text, Color = Theme.Foreground };
            headerBackground = new Background
            {
                Content = new Margin
                {
                    Offset = new Offset(1, 0, 1, 0),
                    Content = textBlock
                }
            };

            Content = headerBackground;
        }

        public void UpdateText(string text)
        {
            textBlock.Text = text;
        }

        public void UpdateTabColor(Color color)
        {
            textBlock.Color = color;
        }

        public void MarkAsActive()
        {
            headerBackground.Color = Theme.TabActive;
        }

        public void MarkAsInactive()
        {
            headerBackground.Color = Theme.TabInactive;
        }

        public void MarkAsUnread()
        {
            headerBackground.Color = Theme.TabAttentionNeeded;
        }
    }
}