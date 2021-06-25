using System;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Space;

namespace Lanchat.Terminal.UserInterface
{
    public class Tab
    {
        private Background headerBackground;
        public IControl Header { get; private set; }
        public IControl Content { get; }
        public VerticalScrollPanel VerticalScrollPanel { get; }

        public Tab(string name, IControl content)
        {
            VerticalScrollPanel = new VerticalScrollPanel
            {
                Content = content,
                ScrollBarBackground = new Character(),
                ScrollBarForeground = new Character(),
                ScrollUpKey = ConsoleKey.PageUp,
                ScrollDownKey = ConsoleKey.PageDown
            };

            UpdateHeader(name);
            Content = VerticalScrollPanel;
            MarkAsInactive();
        }

        public void MarkAsActive() => headerBackground.Color = ConsoleColor.Blue;
        public void MarkAsInactive() => headerBackground.Color = ConsoleColor.Black;

        public void UpdateHeader(string name)
        {
            headerBackground = new Background
            {
                Content = new Margin
                {
                    Offset = new Offset(1, 0, 1, 0),
                    Content = new TextBlock {Text = name}
                }
            };

            Header = headerBackground;
        }
    }
}