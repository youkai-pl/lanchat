using System;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Probe.Handlers
{
    public class EventsHandlers
    {
        private readonly INode node;

        public EventsHandlers(INode node)
        {
            this.node = node;
            node.MessageReceived += OnMessageReceived;
            node.Connected += OnSessionConnected;
            node.Disconnected += OnSessionDisconnected;
            node.SocketErrored += OnSocketErrored;
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Console.WriteLine("Connection error");
            Console.WriteLine($"{node.Id} / {e}");
            Console.WriteLine("");
        }

        private void OnSessionDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.WriteLine($"{node.Id}");
            Console.WriteLine("");
        }

        private void OnSessionConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            Console.WriteLine($"{node.Id} / {node.Endpoint}");
            Console.WriteLine("");
        }

        private void OnMessageReceived(object sender, string e)
        {
            Console.WriteLine("Received");
            Console.WriteLine($"{node.Id} / {node.Endpoint}");
            Console.WriteLine(e);
            Console.WriteLine("");
        }
    }
}