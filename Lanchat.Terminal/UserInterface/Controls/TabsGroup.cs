using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class TabsGroup
    {
        public VerticalStackPanel Headers { get; } = new();
        public List<Tab> Tabs { get; } = new();

        public void AddTab(Tab tab)
        {
            Window.UiAction(() =>
            {
                Tabs.Add(tab);
                Headers.Add(tab.Header);
            });
        }

        public void RemoveTab(Tab tab)
        {
            Window.UiAction(() =>
            {
                Tabs.Remove(tab);
                Headers.Children = Headers.Children.Where(x => x != tab.Header).ToList();
            });
        }

        public void ReplaceTab(Tab previousTab, Tab newTab)
        {
            Window.UiAction(() =>
            {
                Tabs[Tabs.FindIndex(x => x.Equals(previousTab))] = newTab;
                var updatedHeaders = Headers.Children.ToList();
                updatedHeaders[updatedHeaders.IndexOf(previousTab.Header)] = newTab.Header;
                Headers.Children = updatedHeaders.ToList();
            });
        }
    }
}