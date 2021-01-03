using System;
using GLib;
using Lanchat.Gtk.Windows;
using Application = Gtk.Application;

namespace Lanchat.Gtk
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();

            var app = new Application("org.Lanchat.Gtk.Lanchat.Gtk", ApplicationFlags.None);
            app.Register(Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}