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
        public Guid Id { get; init; }

        public Tab(string name, IControl content)
        {
            Content = content;
            UpdateHeader(name);
            MarkAsInactive();
        }

        public void MarkAsActive() => headerBackground.Color = ConsoleColor.Blue;
        public void MarkAsInactive() => headerBackground.Color = ConsoleColor.Black;

        private void UpdateHeader(string name)
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