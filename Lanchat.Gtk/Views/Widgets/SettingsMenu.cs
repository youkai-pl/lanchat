using System;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Views.Widgets
{
    public class SettingsMenu
    {
        [UI] private readonly Popover menu;
        [UI] private readonly Entry nickname;
        [UI] private readonly Button saveButton;
        [UI] private readonly ToggleButton toggle;

        public SettingsMenu(Popover menu, ToggleButton toggle, Entry nickname, Button saveButton)
        {
            this.menu = menu;
            this.toggle = toggle;
            this.nickname = nickname;
            this.saveButton = saveButton;

            nickname.Text = Program.Config.Nickname;

            menu.Closed += OnClosed;
            toggle.Toggled += ToggleOnToggled;
            saveButton.Clicked += SaveButtonOnClicked;
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