using Gtk;
using Lanchat.Gtk.Views.Widgets;
using UI = Gtk.Builder.ObjectAttribute;

namespace Lanchat.Gtk.Views
{
    public class MainWindow : Window
    {
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            DeleteEvent += Window_DeleteEvent;
            builder.Autoconnect(this);

            Chat = new Chat(scroll, chat, input);
            SideBar = new SideBar(sideBar, connectedList, onlineList);

            _ = new SettingsMenu(
                this,
                settingsMenu,
                menuToggle,
                menuNicknameField,
                menuSaveButton,
                sidebarSwitch);

            _ = new ConnectMenu(
                connectMenu,
                connectMenuToggle,
                connectIpAddress,
                connectPortNumber,
                connectButton);

            Program.Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, this); };
        }

        public Chat Chat { get; }
        public SideBar SideBar { get; }

        private static void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
        
#pragma warning disable 649
        // Chat
        [UI] private ScrolledWindow scroll;
        [UI] private ListBox chat;
        [UI] private Entry input;

        // Settings menu
        [UI] private Popover settingsMenu;
        [UI] private ToggleButton menuToggle;
        [UI] private Entry menuNicknameField;
        [UI] private Button menuSaveButton;
        [UI] private CheckButton sidebarSwitch;

        // Connect menu
        [UI] private Popover connectMenu;
        [UI] private ToggleButton connectMenuToggle;
        [UI] private Entry connectIpAddress;
        [UI] private Entry connectPortNumber;
        [UI] private Button connectButton;

        // Sidebar
        [UI] private ScrolledWindow sideBar;
        [UI] private ListBox connectedList;
        [UI] private ListBox onlineList;
#pragma warning restore 649
    }
}