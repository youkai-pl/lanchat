using System;
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
        [UI] private TextView log;
        [UI] private Entry input;
        
        // Settings menu
        [UI] private Popover menu;
        [UI] private ToggleButton menuToggle;
        [UI] private Entry menuNicknameField;
        [UI] private Button menuSaveButton;
#pragma warning restore 649
        
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            input.KeyReleaseEvent += InputOnKeyReleaseEvent;
            menuSaveButton.Clicked += MenuSaveButtonOnClicked;
            menuToggle.Toggled += MenuToggleOnToggled;
            menu.Closed += MenuOnClosed;

            menuNicknameField.Text = Program.Config.Nickname;
            Program.Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, log); };
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

        private void InputOnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (args.Event.Key != Key.Return) return;
            log.Buffer.Text += input.Text + "\n";
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