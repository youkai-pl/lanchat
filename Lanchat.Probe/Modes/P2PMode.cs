using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class P2PMode
    {
        public P2PMode()
        {
            Config.Nickname = "P2P";
            var network = new P2P(3645);
            _ = new ServerEventsHandlers(network.Server);
            network.Server.Start();

            while (true)
            {
                Console.Write("IP Address or message: ");
                var input = Console.ReadLine();

                if (IPAddress.TryParse(input!, out _))
                {
                    var client = network.Connect(input);
                    _ = new NodeEventsHandlers(client);
                }
                else if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                else
                {
                    network.BroadcastMessage(input);
                }

                Console.WriteLine(network.OutgoingConnections.Count);
            }
        }
    }
}