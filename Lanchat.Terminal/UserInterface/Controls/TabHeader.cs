using System;
using ConsoleGUI.Controls;
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
            textBlock = new TextBlock { Text = text };
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

        public void MarkAsActive()
        {
            headerBackground.Color = ConsoleColor.Blue;
        }

        public void MarkAsInactive()
        {
            headerBackground.Color = ConsoleColor.Black;
        }

        public void MarkAsUnread()
        {
            headerBackground.Color = ConsoleColor.DarkMagenta;
        }
    }
}