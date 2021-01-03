using System;
using Gtk;
using Lanchat.Gtk.Windows;

namespace Lanchat.Gtk
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.Lanchat.Gtk.Lanchat.Gtk", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}