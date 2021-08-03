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
        private readonly DockPanel wrapper;
        private int tabSwitch;

        public TabPanel(List<IInputListener> inputListeners)
        {
            this.inputListeners = inputListeners;

            SystemTabs = new TabsGroup(this);
            ChatTabs = new TabsGroup(this);

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
                            Content = SystemTabs.Headers
                        },
                        FillingControl = new Border
                        {
                            BorderStyle = BorderStyle.Single,
                            Content = ChatTabs.Headers
                        }
                    }
                }
            };
            Content = wrapper;
        }

        public Tab CurrentTab { get; private set; }

        public TabsGroup SystemTabs { get; }
        public TabsGroup ChatTabs { get; }

        public List<Tab> AllTabs => SystemTabs.Tabs.Concat(ChatTabs.Tabs).ToList();

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Tab)
            {
                return;
            }

            if ((inputEvent.Key.Modifiers & ConsoleModifiers.Shift) == 0)
            {
                tabSwitch++;
                if (tabSwitch == AllTabs.Count)
                {
                    tabSwitch = 0;
                }
            }
            else
            {
                tabSwitch--;
                if (tabSwitch == -1)
                {
                    tabSwitch = AllTabs.Count - 1;
                }
            }

            SelectTab(AllTabs[tabSwitch]);
            inputEvent.Handled = true;
        }

        public void SelectTab(Tab tab)
        {
            Window.UiAction(() =>
            {
                if (CurrentTab is { Content: IScrollable previousScrollPanel })
                {
                    inputListeners.Remove(previousScrollPanel.ScrollPanel);
                }

                CurrentTab?.Header.MarkAsInactive();
                CurrentTab = tab;

                CurrentTab.Header.MarkAsActive();
                wrapper.FillingControl = new Border
                {
                    BorderStyle = BorderStyle.Single,
                    Content = CurrentTab.Content
                };

                if (CurrentTab.Content is not IScrollable newScrollPanel)
                {
                    return;
                }

                inputListeners.Add(newScrollPanel.ScrollPanel);
                newScrollPanel.ScrollPanel.Top = int.MaxValue;
            });
        }
    }
}