using System;
using Lanchat.Core;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe
{
    public class ClientMode
    {
        public ClientMode(string ipAddress, int port)
        {
            var client = new Client(ipAddress, port);
            _ = new ClientEventsHandlers(client);
            
            Console.WriteLine($"Client connecting to {ipAddress}");
            client.ConnectAsync();

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                client.SendAsync(input);
            }

            Console.WriteLine("Stopping");
            client.DisconnectAndStop();
        }
    }
}