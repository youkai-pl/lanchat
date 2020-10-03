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

            if (!IPAddress.TryParse(ipAddress!, out _))
            {
                ipAddress = "127.0.0.1";
            }

            var client = new Client(ipAddress, port);
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
                        node.SendPing();
                        break;

                    default:
                        node.SendMessage(input);
                        break;
                }
            }

            Console.WriteLine("Stopping");
            client.DisconnectAndStop();
        }
    }
}