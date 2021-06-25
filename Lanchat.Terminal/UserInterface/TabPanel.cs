using System;
using System.Collections.Generic;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface
{
    public class TabPanel : SimpleControl, IInputListener
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