using System;
using GLib;
using Lanchat.ClientCore;
using Lanchat.Core;
using Lanchat.Gtk.Views;
using Application = Gtk.Application;

namespace Lanchat.Gtk
{
    public static class Program
    {
        public static P2P Network { get; private set; }
        public static Config Config { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.Lanchat.Gtk.Lanchat.Gtk", ApplicationFlags.None);
            app.Register(Cancellable.Current);

            Config = Config.Load();
            Network = new P2P();

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Network.StartServer();
            LoggingService.StartLogging();
            LoggingService.CleanLogs();
            Application.Run();
        }
    }
}