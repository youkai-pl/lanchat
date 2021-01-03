using Gtk;
using Lanchat.Core;
using Key = Gdk.Key;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Windows
{
    internal class MainWindow : Window
    {
        [UI] private Entry _input;
        [UI] private TextView _log;

        private readonly P2P network;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;

            _input.KeyReleaseEvent += InputOnKeyReleaseEvent;

            network = new P2P();
            CoreConfig.Nickname = "test";
            
            network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, _log); };
            network.StartServer();
        }


        private void InputOnKeyReleaseEvent(object o, KeyReleaseEventArgs args)
        {
            if (args.Event.Key != Key.Return) return;
            _log.Buffer.Text += _input.Text + "\n";
            network.BroadcastMessage(_input.Text);
            _input.Text = string.Empty;
        }

        private static void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}