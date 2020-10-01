using System;
using System.Net.Sockets;
using Lanchat.Core.Network;

namespace Lanchat.Probe.Handlers
{
    public class EventsHandlers
    {
        private readonly INetworkElement networkElement;

        public EventsHandlers(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
            networkElement.MessageReceived += OnMessageReceived;
            networkElement.Connected += OnSessionConnected;
            networkElement.Disconnected += OnSessionDisconnected;
            networkElement.SocketErrored += OnSocketErrored;
        }

        private void OnSocketErrored(object sender, SocketError e)
        {
            Console.WriteLine("Connection error");
            Console.WriteLine($"{networkElement.Id} / {e}");
            Console.WriteLine("");
        }

        private void OnSessionDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.WriteLine($"{networkElement.Id}");
            Console.WriteLine("");
        }

        private void OnSessionConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            Console.WriteLine($"{networkElement.Id} / {networkElement.Endpoint}");
            Console.WriteLine("");
        }

        private void OnMessageReceived(object sender, string e)
        {
            Console.WriteLine("Received");
            Console.WriteLine($"{networkElement.Id} / {networkElement.Endpoint}");
            Console.WriteLine(e);
            Console.WriteLine("");
        }
    }
}