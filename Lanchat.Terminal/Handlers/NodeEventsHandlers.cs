using System;
using System.Diagnostics;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Terminal.Ui;

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
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Prompt.Log.Add($"{node.Nickname} connected");
            Prompt.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Prompt.Log.Add($"{node.Nickname} disconnected. Trying reconnect.");
            Prompt.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }
        
        private void OnHardDisconnected(object sender, EventArgs e)
        {
            Prompt.Log.Add($"{node.Nickname} disconnected. Cannot reconnect.");
            Prompt.NodesCount.Text = Program.Network.Nodes.Count.ToString();
        }
        
        private void OnMessageReceived(object sender, string e)
        {
            Prompt.Log.AddMessage(e, node.Nickname);
        }
        
        private void OnPingReceived(object sender, EventArgs e)
        {
            Prompt.Log.Add($"{node.Nickname} sent ping");
        }
        
        private void OnSocketErrored(object sender, SocketError e)
        {
            Trace.WriteLine($"[NODE] Socket error {node.Id} / {e}");
            Prompt.Log.Add($"Connection error {e}");
        }
    }
}