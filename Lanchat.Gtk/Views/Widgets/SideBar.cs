using System;
using System.Linq;
using Gtk;

namespace Lanchat.Gtk.Views.Widgets
{
    public class SideBar
    {
        private readonly ListBox connectedList;
        private readonly ListBox onlineList;
        private readonly ScrolledWindow sideBar;

        public SideBar(MainWindow mainWindow)
        {
            sideBar = mainWindow.SideBar;
            connectedList = mainWindow.ConnectedList;
            onlineList = mainWindow.OnlineList;
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
                sideBar.Hide();
            else
                sideBar.Show();
        }
    }
}