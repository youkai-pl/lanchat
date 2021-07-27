using System;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Space;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class Tab
    {
        public Tab(string name, IControl content)
        {
            Content = content;
            Header = new TabHeader(name);
            Header.MarkAsInactive();
        }

        public TabHeader Header { get; }
        public IControl Content { get; }
        public Guid Id { get; init; }
    }
}