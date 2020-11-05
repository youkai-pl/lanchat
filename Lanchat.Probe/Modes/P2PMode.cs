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
            CoreConfig.Nickname = "P2P";
            
            var network = new P2P();
            network.ConnectionCreated += (sender, node) => { _ = new NodeEventsHandlers(node); };

            network.StartServer();

            while (true)
            {
                Console.Write("IP Address or message: ");
                var input = Console.ReadLine();

                if (IPAddress.TryParse(input!, out var parsedIp))
                {
                    network.Connect(parsedIp);
                }
                else if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                else
                {
                    network.BroadcastMessage(input);
                }
            }
        }
    }
}