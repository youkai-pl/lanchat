using System;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;

namespace Lanchat.Terminal.UserInterface
{
    public partial class TabPanel
    {
        public class Tab
        {
            private readonly Background headerBackground;

            public IControl Header { get; }
            public IControl Content { get; }

            public Tab(string name, IControl content)
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
                Content = content;

                MarkAsInactive();
            }

            public void MarkAsActive() => headerBackground.Color = ConsoleColor.Blue;
            public void MarkAsInactive() => headerBackground.Color = ConsoleColor.DarkBlue;
        }
    }
}