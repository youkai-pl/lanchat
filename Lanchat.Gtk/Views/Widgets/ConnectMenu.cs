using System;
using System.Net;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Views.Widgets
{
    public class ConnectMenu
    {
        [UI] private readonly Entry ipAddressEntry;
        [UI] private readonly Popover menu;
        [UI] private readonly Entry portNumberEntry;
        [UI] private readonly ToggleButton toggle;
        [UI] private Button connectButton;

        public ConnectMenu(Popover menu, ToggleButton toggle, Entry ipAddressEntry,
            Entry portNumberEntry, Button connectButton)
        {
            this.menu = menu;
            this.toggle = toggle;
            this.ipAddressEntry = ipAddressEntry;
            this.portNumberEntry = portNumberEntry;
            this.connectButton = connectButton;

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