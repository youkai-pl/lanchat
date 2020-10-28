using System;
using System.Diagnostics;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Handlers
{
    public class NodeEventsHandlers
    {
        private readonly Node node;

        public NodeEventsHandlers(Node node)
        {
            this.node = node;
            node.NetworkInput.MessageReceived += OnMessageReceived;
            node.NetworkInput.PingReceived += OnPingReceived;
            node.Connected += OnConnected;
            node.Disconnected += OnDisconnected;
            node.HardDisconnect += OnHardDisconnected;
            node.SocketErrored += OnSocketErrored;
            node.NicknameChanged += OnNicknameChanged;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} connected");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} disconnected. Trying reconnect.");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }
        
        private void OnHardDisconnected(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} disconnected. Cannot reconnect.");
            Ui.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }
        
        private void OnMessageReceived(object sender, string e)
        {
            Ui.Log.AddMessage(e, node.Nickname);
        }
        
        private void OnPingReceived(object sender, EventArgs e)
        {
            Ui.Log.Add($"{node.Nickname} sent ping");
        }
        
        private void OnSocketErrored(object sender, SocketError e)
        {
            Trace.WriteLine($"[NODE] Socket error {node.Id} / {e}");
            Ui.Log.Add($"Connection error {e}");
        }
        
        private void OnNicknameChanged(object sender, string e)
        {
            Ui.Log.Add($"{e} changed nickname to {node.Nickname}");
        }
    }
}