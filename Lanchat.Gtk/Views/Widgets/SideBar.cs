using System;
using System.ComponentModel;
using System.Linq;
using Gtk;
using Lanchat.Core.Models;

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

            Program.Network.Broadcasting.NodeDetected += NetworkOnNodeDetected;
            Program.Network.Broadcasting.DetectedNodeDisappeared += NetworkOnDetectedNodeDisappeared;
        }

        private void NetworkOnNodeDetected(object sender, Broadcast e)
        {
            Application.Invoke(delegate
            {
                onlineList.Add(new ListBoxRow
                {
                    Child = new Label(e.Nickname)
                    {
                        Margin = 2
                    },
                    Halign = Align.Start,
                    Name = $"{e.Guid}-ol"
                });
                onlineList.ShowAll();
            });
            
            e.PropertyChanged += EOnPropertyChanged;
        }

        private void EOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName != "Nickname") return;

            var broadcast = (Broadcast) sender;
            
            Application.Invoke(delegate
            {
                var row = (ListBoxRow) onlineList.Children.FirstOrDefault(x => x.Name == $"{broadcast.Guid}-ol");
                var label = (Label) row?.Child;
                if (label != null) label.Text = broadcast.Nickname;
            });
        }
        
        private void NetworkOnDetectedNodeDisappeared(object sender, Broadcast e)
        {
            Application.Invoke(delegate
            {
                onlineList.Remove(onlineList.Children.FirstOrDefault(x => x.Name == $"{e.Guid}-ol"));
            });
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