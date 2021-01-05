using System;
using System.Linq;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Views.Widgets
{
    public class SideBar
    {
        [UI] private readonly ScrolledWindow sideBar;
        [UI] private readonly ListBox connectedList;
        [UI] private readonly ListBox onlineList;

        public SideBar(ScrolledWindow sideBar, ListBox connectedList, ListBox onlineList)
        {
            this.sideBar = sideBar;
            this.connectedList = connectedList;
            this.onlineList = onlineList;
        }

        public void AddConnected(string nickname, Guid id)
        {
            Application.Invoke(delegate
            {
                connectedList.Add(new ListBoxRow
                {
                    Child = new Label(nickname)
                    {
                        Margin = 2
                    },
                    Halign = Align.Start,
                    Name = $"{id}-cl"
                });
                connectedList.ShowAll();
            });
        }

        public void RemoveConnected(Guid id)
        {
            Application.Invoke(delegate
            {
                connectedList.Remove(connectedList.Children.FirstOrDefault(x => x.Name == $"{id}-cl"));
            });
        }

        public void SwitchVisibility()
        {
            if (sideBar.Visible)
            {
                sideBar.Hide();
            }
            else
            {
                sideBar.Show();
            }
        }
    }
}