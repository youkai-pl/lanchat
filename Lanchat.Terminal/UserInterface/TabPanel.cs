using System;
using System.Collections.Generic;
using ConsoleGUI;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public partial class TabPanel : SimpleControl, IInputListener
    {
        private readonly List<IInputListener> inputListeners;
        private readonly List<Tab> tabs = new();
        private readonly DockPanel wrapper;
        private readonly HorizontalStackPanel tabsPanel;

        public Tab CurrentTab { get; private set; }

        public TabPanel(List<IInputListener> inputListeners)
        {
            this.inputListeners = inputListeners;
            tabsPanel = new HorizontalStackPanel();

            wrapper = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Top,
                DockedControl = new Background
                {
                    Color = ConsoleColor.DarkBlue,
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
            // TODO: Handle tabs where content is not IInputListener
            try
            {
                inputListeners.Remove((IInputListener) CurrentTab.Content);
            }
            catch (NullReferenceException)
            { }

            CurrentTab?.MarkAsInactive();
            CurrentTab = tabs[tab];

            inputListeners.Add((IInputListener) CurrentTab.Content);
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