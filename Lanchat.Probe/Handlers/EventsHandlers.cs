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
            networkElement.MessageReceived += SessionOnMessageReceived;
            networkElement.Connected += SessionOnSessionConnected;
            networkElement.Disconnected += SessionOnSessionDisconnected;
            networkElement.SocketErrored += SocketOnSocketErrored;
        }

        private void SocketOnSocketErrored(object sender, SocketError e)
        {
            Console.WriteLine("Connection error");
            Console.WriteLine($"{networkElement.GetId()} / {e}");
            Console.WriteLine("");
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
            Console.WriteLine($"{networkElement.GetId()}");
            Console.WriteLine("");
        }

        private void SessionOnSessionConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
            Console.WriteLine($"{networkElement.GetId()} / {networkElement.GetEndPoint()}");
            Console.WriteLine("");
        }

        private void SessionOnMessageReceived(object sender, string e)
        {
            Console.WriteLine("Received");
            Console.WriteLine($"{networkElement.GetId()} / {networkElement.GetEndPoint()}");
            Console.WriteLine(e);
            Console.WriteLine("");
        }
    }
}