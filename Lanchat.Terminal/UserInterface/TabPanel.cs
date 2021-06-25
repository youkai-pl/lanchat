using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class TabPanel : SimpleControl, IInputListener
    {
        private readonly List<IInputListener> inputListeners;
        private readonly List<Tab> tabs = new();
        private readonly DockPanel wrapper;
        private readonly VerticalStackPanel tabsPanel;

        public Tab CurrentTab { get; private set; }

        public TabPanel(List<IInputListener> inputListeners)
        {
            this.inputListeners = inputListeners;
            tabsPanel = new VerticalStackPanel();

            wrapper = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Right,
                DockedControl = new Border
                {
                    BorderStyle = BorderStyle.Single,
                    Content = new Boundary
                    {
                        MinWidth = 25,
                        MaxWidth = 25,
                        Content = tabsPanel
                    }
                }
            };

            Content = wrapper;
        }

        public void AddTab(Tab tab)
        {
            tabs.Add(tab);
            tabsPanel.Add(tab.Header);
            if (tabs.Count == 1)
                SelectTab(0);
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Tab) return;

            SelectTab((tabs.IndexOf(CurrentTab) + 1) % tabs.Count);
            inputEvent.Handled = true;
        }

        private void SelectTab(int tab)
        {
            if (CurrentTab is {Content: IInputListener oldTabListener})
            {
                inputListeners.Remove(oldTabListener);
            }

            CurrentTab?.MarkAsInactive();
            CurrentTab = tabs[tab];

            if (CurrentTab.Content is IInputListener newTabListener)
            {
                inputListeners.Add(newTabListener);
            }

            CurrentTab.MarkAsActive();
            wrapper.FillingControl = CurrentTab.Content;
        }
    }
}