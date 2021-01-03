using System;
using System.Net;
using Gtk;
using Key = Gdk.Key;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Windows
{
    internal class MainWindow : Window
    {
#pragma warning disable 649
        // Main content
        [UI] private ScrolledWindow scroll;
        [UI] private ListBox chat;
        [UI] private Entry input;

        // Settings menu
        [UI] private Popover menu;
        [UI] private ToggleButton menuToggle;
        [UI] private Entry menuNicknameField;
        [UI] private Button menuSaveButton;

        // Connect menu
        [UI] private Popover connectMenu;
        [UI] private ToggleButton connectMenuToggle;
        [UI] private Entry connectIpAddress;
        [UI] private Entry connectPortNumber;
        [UI] private Button connectButton;
#pragma warning restore 649

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            input.KeyReleaseEvent += InputOnKeyReleaseEvent;
            menu.Closed += MenuOnClosed;
            menuToggle.Toggled += MenuToggleOnToggled;
            menuSaveButton.Clicked += MenuSaveButtonOnClicked;
            connectMenu.Closed += ConnectMenuOnClosed;
            connectMenuToggle.Toggled += ConnectMenuToggleOnToggled;
            connectButton.Clicked += ConnectButtonOnClicked;

            menuNicknameField.Text = Program.Config.Nickname;
            connectPortNumber.Text = Program.Config.Port.ToString();
            Program.Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, chat); };
        }

        // UI Events
        private void MenuToggleOnToggled(object sender, EventArgs e)
        {
            if (menuToggle.Active) menu.ShowAll();
        }

        private void MenuOnClosed(object sender, EventArgs e)
        {
            menuToggle.Active = false;
        }

        private void MenuSaveButtonOnClicked(object sender, EventArgs e)
        {
            Program.Config.Nickname = menuNicknameField.Text;
        }

        private void ConnectMenuOnClosed(object sender, EventArgs e)
        {
            connectMenuToggle.Active = false;
        }

        private void ConnectMenuToggleOnToggled(object sender, EventArgs e)
        {
            if (connectMenuToggle.Active) connectMenu.ShowAll();
        }

        private void ConnectButtonOnClicked(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(connectIpAddress.Text, out var ipAddress))
            {
                Program.Network.Connect(ipAddress, int.Parse(connectPortNumber.Text));
                connectMenu.Hide();
            }
        }

        private void InputOnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (args.Event.Key != Key.Return) return;
            
            chat.Add(new Label
            {
                Text = $"{Program.Config.Nickname}: {input.Text}",
            });
            chat.ShowAll();
            
            scroll.Vadjustment.Value = double.MaxValue;
            Program.Network.BroadcastMessage(input.Text);
            input.Text = string.Empty;
        }

        private static void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}