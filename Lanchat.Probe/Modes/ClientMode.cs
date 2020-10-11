using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Core.Network;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class ClientMode
    {
        public ClientMode(int port)
        {
            Config.Nickname = "Client";
            Console.Write("IP Address (leave blank to localhost): ");
            var ipAddress = Console.ReadLine();

            if (!IPAddress.TryParse(ipAddress!, out var parsedIp))
            {
                parsedIp = IPAddress.Loopback;
            }

            var client = new Client(parsedIp, port);
            var node = new Node(client);
            _ = new NodeEventsHandlers(node);

            Console.WriteLine($"Client connecting to {ipAddress}");
            client.ConnectAsync();

            var loop = true;
            while (loop)
            {
                var input = Console.ReadLine();

                switch (input)
                {
                    case "/q":
                        loop = false;
                        break;

                    case "/p":
                        node.NetworkOutput.SendPing();
                        break;

                    default:
                        node.NetworkOutput.SendMessage(input);
                        break;
                }
            }

            Console.WriteLine("Stopping");
            client.Close();
        }
    }
}