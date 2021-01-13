using System;
using Gtk;

namespace Lanchat.Gtk.Views.Widgets
{
    public class SettingsMenu
    {
        private readonly MainWindow mainWindow;
        private readonly Popover menu;
        private readonly Entry nickname;
        private readonly Button saveButton;
        private readonly CheckButton sidebarSwitch;
        private readonly ToggleButton toggle;

        public SettingsMenu(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            menu = mainWindow.Menu;
            toggle = mainWindow.MenuToggle;
            nickname = mainWindow.MenuNicknameField;
            saveButton = mainWindow.MenuSaveButton;
            sidebarSwitch = mainWindow.SidebarSwitch;

            nickname.Text = Program.Config.Nickname;

            menu.Closed += OnClosed;
            toggle.Toggled += ToggleOnToggled;
            saveButton.Clicked += SaveButtonOnClicked;
            sidebarSwitch.Clicked += SidebarSwitchOnClicked;
        }

        private void SidebarSwitchOnClicked(object sender, EventArgs e)
        {
            mainWindow.SideBarWidget.SwitchVisibility();
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