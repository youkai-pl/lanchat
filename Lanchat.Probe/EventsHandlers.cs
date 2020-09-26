using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe
{
    public class EventsHandlers
    {
        public EventsHandlers(Events events)
        {
            events.ClientConnected += EventsOnClientConnected;
            events.ClientDisconnected += EventsOnClientDisconnected;
            events.ClientError += EventsOnClientError;
            events.MessageReceived += EventsOnMessageReceived;
            events.ServerError += EventsOnServerError;
        }

        private static void EventsOnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client connected");
        }

        private static void EventsOnClientDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client disconnected");
        }

        private static void EventsOnClientError(object sender, SocketError e)
        {
            Console.WriteLine($"Client returned error: {e}");
        }

        private static void EventsOnMessageReceived(object sender, string e)
        {
            Console.WriteLine($"Message received: {e}");
        }

        private static void EventsOnServerError(object sender, SocketError e)
        {
            Console.WriteLine($"Server returned error: {e}");
        }
    }
}