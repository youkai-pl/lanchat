using System;
using System.Collections.Generic;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.Space;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class TabPanel : SimpleControl, IInputListener
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

            public void MarkAsActive() => headerBackground.Color = ConsoleColor.DarkBlue;
            public void MarkAsInactive() => headerBackground.Color = ConsoleColor.Blue;
        }

        private readonly List<Tab> tabs = new();
        private readonly DockPanel wrapper;
        private readonly HorizontalStackPanel tabsPanel;

        public Tab CurrentTab { get; private set; }

        public TabPanel()
        {
            tabsPanel = new HorizontalStackPanel();

            wrapper = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Top,
                DockedControl = new Background
                {
                    Color = ConsoleColor.Blue,
                    Content = new Boundary
                    {
                        MinHeight = 1,
                        MaxHeight = 1,
                        Content = tabsPanel
                    }
                }
            };

            Content = wrapper;
        }

        public void AddTab(string name, IControl content)
        {
            var newTab = new Tab(name, content);
            tabs.Add(newTab);
            tabsPanel.Add(newTab.Header);
            if (tabs.Count == 1)
                SelectTab(0);
        }

        private void SelectTab(int tab)
        {
            CurrentTab?.MarkAsInactive();
            CurrentTab = tabs[tab];
            CurrentTab.MarkAsActive();
            wrapper.FillingControl = CurrentTab.Content;
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Tab) return;

            SelectTab((tabs.IndexOf(CurrentTab) + 1) % tabs.Count);
            inputEvent.Handled = true;
        }
    }
}