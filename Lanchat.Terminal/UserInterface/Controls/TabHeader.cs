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
            textBlock = new TextBlock
            {
                Text = text,
                Color = Theme.TabActiveText
            };
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
            textBlock.Color = Theme.TabActiveText;
            headerBackground.Color = Theme.TabActive;
        }

        public void MarkAsInactive()
        {
            textBlock.Color = Theme.Foreground;
            headerBackground.Color = Theme.Background;
        }

        public void MarkAsUnread()
        {
            textBlock.Color = Theme.TabAttentionNeededText;
            headerBackground.Color = Theme.TabAttentionNeeded;
        }
    }
}