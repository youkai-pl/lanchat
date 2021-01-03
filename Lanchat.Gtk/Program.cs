using System;
using GLib;
using Lanchat.ClientCore;
using Lanchat.Core;
using Lanchat.Gtk.Windows;
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
            Application.Run();
            
            Network.StartServer();
            LoggingService.StartLogging();
            LoggingService.CleanLogs();
        }
    }
}