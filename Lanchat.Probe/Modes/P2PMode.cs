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
            network.ConnectionCreated += (sender, node) =>
            {
                _ = new NodeEventsHandlers(node);
            };
            
            network.Start();

            while (true)
            {
                Console.Write("IP Address or message: ");
                var input = Console.ReadLine();

                if (IPAddress.TryParse(input!, out _))
                {
                    network.Connect(input);
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