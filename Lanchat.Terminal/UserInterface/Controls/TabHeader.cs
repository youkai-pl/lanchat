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
        private Color? lockedColor;
        private bool isActive;

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
            lockedColor = color;
            if (isActive)
            {
                textBlock.Color = Theme.TabActiveText;
                headerBackground.Color = color;
            }
            else
            {
                textBlock.Color = color;
                headerBackground.Color = Theme.Background;
            }
        }

        public void MarkAsActive()
        {
            isActive = true;
            headerBackground.Color = lockedColor ?? Theme.TabActive;
            textBlock.Color = Theme.TabActiveText;
        }

        public void MarkAsInactive()
        {
            isActive = false;
            textBlock.Color = lockedColor ?? Theme.Foreground;
            headerBackground.Color = Theme.Background;
        }

        public void MarkAsUnread()
        {
            textBlock.Color =  Theme.TabAttentionNeededText;
            headerBackground.Color = lockedColor ?? Theme.TabAttentionNeeded;
        }

        public void ResetColor()
        {
            lockedColor = null;
            if (isActive)
            {
                MarkAsActive();
            }
            else
            {
                MarkAsInactive();
            }
        }
    }
}