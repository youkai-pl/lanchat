using System;
using System.Net;
using Lanchat.Core;
using Lanchat.Probe.Handlers;

namespace Lanchat.Probe
{
    public class ServerMode
    {
        public ServerMode(int port)
        {
            var server = new Server(IPAddress.Any, port);
            _ = new ServerEventsHandlers(server);
            
            server.Start();
            Console.WriteLine($"Server started on port {server.Endpoint.Port}");

            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                Console.WriteLine(server.IncomingConnections.Count);
                server.IncomingConnections.ForEach(x => x.SendMessage(input));
            }

            Console.WriteLine("Stopping");
            server.Stop();
        }
    }
}