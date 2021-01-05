using System;
using System.Net;
using Gtk;

namespace Lanchat.Gtk.Views.Widgets
{
    public class ConnectMenu
    {
        private readonly Entry ipAddressEntry;
        private readonly Popover menu;
        private readonly Entry portNumberEntry;
        private readonly ToggleButton toggle;
        private readonly Button connectButton;

        public ConnectMenu(MainWindow mainWindow)
        {
            menu = mainWindow.ConnectMenu;
            toggle = mainWindow.ConnectMenuToggle;
            ipAddressEntry = mainWindow.ConnectIpAddress;
            portNumberEntry = mainWindow.ConnectPortNumber;
            connectButton = mainWindow.ConnectButton;
            portNumberEntry.Text = Program.Config.Port.ToString();

            menu.Closed += MenuOnClosed;
            toggle.Toggled += ToggleOnToggled;
            connectButton.Clicked += ButtonOnClicked;
        }

        private void MenuOnClosed(object sender, EventArgs e)
        {
            toggle.Active = false;
        }

        private void ToggleOnToggled(object sender, EventArgs e)
        {
            if (toggle.Active) menu.ShowAll();
        }

        private void ButtonOnClicked(object sender, EventArgs e)
        {
            if (!IPAddress.TryParse(ipAddressEntry.Text, out var ipAddress)) return;
            Program.Network.Connect(ipAddress, int.Parse(portNumberEntry.Text));
            menu.Hide();
        }
    }
}