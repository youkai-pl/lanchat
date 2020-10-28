using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Core.Network;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class ClientMode
    {
        public ClientMode()
        {
            CoreConfig.Nickname = "Client";
            Console.Write("IP Address (leave blank to localhost): ");
            var ipAddress = Console.ReadLine();

            if (!IPAddress.TryParse(ipAddress!, out var parsedIp))
            {
                parsedIp = IPAddress.Loopback;
            }

            var client = new Client(parsedIp, CoreConfig.ServerPort);
            var node = new Node(client, false);
            _ = new NodeEventsHandlers(node);

            Console.WriteLine($"Client connecting to {parsedIp}");
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