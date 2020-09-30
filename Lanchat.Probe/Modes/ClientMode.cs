using System;
using System.Net;
using Lanchat.Core.Network;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class ClientMode
    {
        public ClientMode(int port)
        {
            Console.Write("IP Address (leave blank to localhost): ");
            var ipAddress = Console.ReadLine();

            if (!IPAddress.TryParse(ipAddress!, out _))
            {
                ipAddress = "127.0.0.1";
            }

            var client = new Client(ipAddress, port);
            _ = new EventsHandlers(client);

            Console.WriteLine($"Client connecting to {ipAddress}");
            client.ConnectAsync();

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                client.Node.SendMessage(input);
            }

            Console.WriteLine("Stopping");
            client.DisconnectAndStop();
        }
    }
}