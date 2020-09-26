using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe
{
    public class ClientEventsHandlers
    {
        private readonly Client client;
        
        public ClientEventsHandlers(Client client)
        {
            this.client = client;
            client.ClientConnected += ClientOnClientConnected;
            client.ClientDisconnected += ClientOnClientDisconnected;
            client.MessageReceived += ClientOnMessageReceived;
            client.ClientErrored += ClientOnClientErrored;
        }

        private void ClientOnClientErrored(object sender, SocketError e)
        {
            Console.WriteLine($"Client returned error: {e}");
        }

        private void ClientOnMessageReceived(object sender, string e)
        {
            Console.WriteLine($"({client.Endpoint.Address}) {e}");
        }

        private void ClientOnClientDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Client disconnected {client.Endpoint.Address}");
        }

        private void ClientOnClientConnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Client connected {client.Endpoint.Address}");
        }
    }
}