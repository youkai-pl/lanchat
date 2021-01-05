using System;
using System.Diagnostics;
using Gtk;
using Switch = Gtk.Switch;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Views.Widgets
{
    public class SettingsMenu
    {
        private readonly MainWindow mainWindow;
        [UI] private readonly Popover menu;
        [UI] private readonly Entry nickname;
        [UI] private readonly Button saveButton;
        [UI] private readonly ToggleButton toggle;
        [UI] private readonly CheckButton sidebarSwitch;

        public SettingsMenu(MainWindow mainWindow, Popover menu, ToggleButton toggle, Entry nickname, Button saveButton,
            CheckButton sidebarSwitch)
        {
            this.mainWindow = mainWindow;
            this.menu = menu;
            this.toggle = toggle;
            this.nickname = nickname;
            this.saveButton = saveButton;
            this.sidebarSwitch = sidebarSwitch;
            
            nickname.Text = Program.Config.Nickname;

            menu.Closed += OnClosed;
            toggle.Toggled += ToggleOnToggled;
            saveButton.Clicked += SaveButtonOnClicked;
            sidebarSwitch.Clicked += SidebarSwitchOnClicked;
        }

        private void SidebarSwitchOnClicked(object sender, EventArgs e)
        {
            mainWindow.SideBar.SwitchVisibility();
        }

        private void ToggleOnToggled(object sender, EventArgs e)
        {
            if (toggle.Active) menu.ShowAll();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            toggle.Active = false;
        }

        private void SaveButtonOnClicked(object sender, EventArgs e)
        {
            Program.Config.Nickname = nickname.Text;
        }
    }
}