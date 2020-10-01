using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Core.Network;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe.Modes
{
    public class P2PMode
    {
        public P2PMode()
        {
            // ReSharper disable once InconsistentNaming
            var p2p = new P2P(3645);
            _ = new ServerEventsHandlers(p2p.Server);
            p2p.Server.Start();

            while (true)
            {
                Console.Write("IP Address or message: ");
                var input = Console.ReadLine();

                if (IPAddress.TryParse(input!, out _))
                {
                    var client = p2p.Connect(input);
                    _ = new EventsHandlers(client);
                }
                else if (string.IsNullOrEmpty(input))
                {
                    break;
                }
                else
                {
                    p2p.BroadcastMessage(input);
                }

                Console.WriteLine(p2p.OutgoingConnections.Count);
            }
        }
    }
}