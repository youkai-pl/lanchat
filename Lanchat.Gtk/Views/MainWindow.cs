using System.Diagnostics;
using Gtk;
using Lanchat.Gtk.Views.Widgets;
using UI = Gtk.Builder.ObjectAttribute;

// ReSharper disable UnassignedField.Global

namespace Lanchat.Gtk.Views
{
    public class MainWindow : Window
    {
        // Chat
        [UI] public ScrolledWindow Scroll;
        [UI] public ListBox Chat;
        [UI] public Button ConnectButton;
        [UI] public ListBox ConnectedList;
        [UI] public Entry ConnectIpAddress;

        // Connect menu
        [UI] public Popover ConnectMenu;
        [UI] public ToggleButton ConnectMenuToggle;
        [UI] public Entry ConnectPortNumber;
        [UI] public Entry Input;

        // Settings menu
        [UI] public Popover Menu;
        [UI] public Entry MenuNicknameField;
        [UI] public Button MenuSaveButton;
        [UI] public ToggleButton MenuToggle;
        [UI] public ListBox OnlineList;

        // Sidebar
        [UI] public ScrolledWindow SideBar;
        [UI] public CheckButton SidebarSwitch;

        public ChatWidget ChatWidget { get; }
        public SideBar SideBarWidget { get; }
        
        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            DeleteEvent += Window_DeleteEvent;
            builder.Autoconnect(this);

            ChatWidget = new ChatWidget(this);
            SideBarWidget = new SideBar(this);

            _ = new SettingsMenu(this);
            _ = new ConnectMenu(this);

            Program.Network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node, this); };
        }

        private static void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }
    }
}