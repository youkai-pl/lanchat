using Gtk;
using Lanchat.ClientCore;
using Lanchat.Core;
using Key = Gdk.Key;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Windows
{
    internal class MainWindow : Window
    {
        private readonly P2P network;
        public static Config Config { get; private set; }

        // UI elements
#pragma warning disable 649
        [UI] private Entry input;
        [UI] private TextView log;
        [UI] private Popover menu;
        [UI] private ToggleButton menuToggle;
        [UI] private ScrolledWindow scroll;
#pragma warning restore 649


        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;

            input.KeyReleaseEvent += InputOnKeyReleaseEvent;
            menuToggle.Toggled += (o, args) =>
            {
                if (menuToggle.Active) menu.ShowAll();
            };
            menu.Closed += (o, args) => { menuToggle.Active = false; };

            Config = Config.Load();
            network = new P2P();

            network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, log); };
            network.StartServer();
        }

        private void InputOnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (args.Event.Key != Key.Return) return;
            log.Buffer.Text += input.Text + "\n";
            scroll.Vadjustment.Value = double.MaxValue;
            network.BroadcastMessage(input.Text);
            input.Text = string.Empty;
        }

        private static void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}