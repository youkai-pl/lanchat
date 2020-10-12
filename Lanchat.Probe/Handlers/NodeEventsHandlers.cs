using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe.Handlers
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
            node.HardDisconnect += OnHardDisconnect;
            node.SocketErrored += OnSocketErrored;
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Console.WriteLine("Connection error");
            Console.WriteLine($"{node.Id} / {e}");
            Console.WriteLine("");
        }

        private void OnDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected. Trying reconnect.");
            Console.WriteLine($"{node.Id}");
            Console.WriteLine("");
        }

        private void OnHardDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.WriteLine($"{node.Id}");
            Console.WriteLine("");
        }

        private void OnConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            Console.WriteLine($"{node.Id} / {node.Endpoint}");
            Console.WriteLine("");
        }

        private void OnMessageReceived(object sender, string e)
        {
            Console.WriteLine("Message received");
            Console.WriteLine($"{node.Nickname} / {node.Id} / {node.Endpoint}");
            Console.WriteLine(e);
            Console.WriteLine("");
        }

        private void OnPingReceived(object sender, EventArgs e)
        {
            Console.WriteLine("Ping received");
            Console.WriteLine($"{node.Nickname} / {node.Id} / {node.Endpoint}");
            Console.WriteLine("");
        }
    }
}