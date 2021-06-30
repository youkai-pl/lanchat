using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Data;
using ConsoleGUI.Input;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class TabPanel : SimpleControl, IInputListener
    {
        private readonly List<IInputListener> inputListeners;
        private readonly VerticalStackPanel tabsHeaders = new();
        private readonly VerticalStackPanel userTabsHeaders = new();
        private readonly DockPanel wrapper;
        private int tabSwitch;

        public TabPanel(List<IInputListener> inputListeners)
        {
            this.inputListeners = inputListeners;

            wrapper = new DockPanel
            {
                Placement = DockPanel.DockedControlPlacement.Right,
                DockedControl = new Boundary
                {
                    MinWidth = 25,
                    MaxWidth = 25,
                    Content = new DockPanel
                    {
                        DockedControl = new Border
                        {
                            BorderStyle = BorderStyle.Single,
                            Content = tabsHeaders
                        },
                        FillingControl = new Border
                        {
                            BorderStyle = BorderStyle.Single,
                            Content = userTabsHeaders
                        }
                    }
                }
            };
            Content = wrapper;
        }

        public Tab CurrentTab { get; private set; }
        public List<Tab> Tabs { get; } = new();

        public void AddTab(Tab tab)
        {
            Tabs.Add(tab);
            tabsHeaders.Add(tab.Header);
            if (Tabs.Count == 1)
            {
                SelectTab(Tabs[0]);
            }
        }

        public void AddUserTab(Tab tab)
        {
            Tabs.Add(tab);
            userTabsHeaders.Add(tab.Header);
            if (Tabs.Count == 1)
            {
                SelectTab(Tabs[0]);
            }
        }

        public void RemoveUserTab(Tab tab)
        {
            if (CurrentTab == tab)
            {
                SelectTab(Tabs[0]);
            }

            Tabs.Remove(tab);
            userTabsHeaders.Children = userTabsHeaders.Children.Where(x => x != tab.Header).ToList();
        }

        public void ReplaceTab(Tab previousTab, Tab newTab)
        {
            Tabs[Tabs.FindIndex(x => x.Equals(previousTab))] = newTab;
            var newTabsHeaders = tabsHeaders.Children.ToList();
            newTabsHeaders[newTabsHeaders.IndexOf(previousTab.Header)] = newTab.Header;
            tabsHeaders.Children = newTabsHeaders.ToList();
            SelectTab(newTab);
        }
        
        public void UpdateUserTabHeader(Tab tab, string headerText)
        {
            var newUserTabsHeaders = userTabsHeaders.Children.ToList();
            var index = newUserTabsHeaders.IndexOf(tab.Header);
            tab.UpdateHeader(headerText);
            newUserTabsHeaders[index] = tab.Header;
            userTabsHeaders.Children = newUserTabsHeaders.ToList();
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Tab)
            {
                return;
            }

            if ((inputEvent.Key.Modifiers & ConsoleModifiers.Shift) == 0)
            {
                tabSwitch++;
                if (tabSwitch == Tabs.Count)
                {
                    tabSwitch = 0;
                }
            }
            else
            {
                tabSwitch--;
                if (tabSwitch == -1)
                {
                    tabSwitch = Tabs.Count - 1;
                }
            }

            SelectTab(Tabs[tabSwitch]);
            inputEvent.Handled = true;
        }
        
        private void SelectTab(Tab tab)
        {
            if (CurrentTab is {Content: IScrollable previousScrollPanel})
            {
                inputListeners.Remove(previousScrollPanel.ScrollPanel);
            }

            CurrentTab?.MarkAsInactive();
            CurrentTab = tab;

            if (CurrentTab.Content is IScrollable newScrollPanel)
            {
                inputListeners.Add(newScrollPanel.ScrollPanel);
                newScrollPanel.ScrollPanel.Top = int.MaxValue;
            }

            CurrentTab.MarkAsActive();
            wrapper.FillingControl = new Border
            {
                BorderStyle = BorderStyle.Single,
                Content = CurrentTab.Content
            };
        }
    }
}