using System;
using System.Net.Sockets;
using Gtk;
using Lanchat.Gtk.Windows;
using Node = Lanchat.Core.Node;

namespace Lanchat.Gtk
{
    public class NodeEventsHandlers
    {
        private readonly Node node;
        private readonly MainWindow mainWindow;
        private object lockChat = new object();

        public NodeEventsHandlers(Node node, MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.NetworkInput.PrivateMessageReceived += OnPrivateMessageReceived;
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
            node.SocketErrored += OnSocketErrored;
            node.NicknameChanged += OnNicknameChanged;
            node.CannotConnect += OnCannotConnect;
        }

        private void OnConnected(object sender, EventArgs e)
        {
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
        }

        private void OnHardDisconnected(object sender, EventArgs e)
        {
        }

        private void OnMessageReceived(object sender, string e)
        {
            mainWindow.AddChatEntry(node.Nickname, e);
        }

        private void OnPrivateMessageReceived(object sender, string e)
        {
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
        }

        private void OnNicknameChanged(object sender, string e)
        {
        }

        private void OnCannotConnect(object sender, EventArgs e)
        {
        }
    }
}