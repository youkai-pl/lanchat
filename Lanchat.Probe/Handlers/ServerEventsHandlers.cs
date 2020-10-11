using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe.Handlers
{
    public class ServerEventsHandlers
    {
        public ServerEventsHandlers(Server server)
        {
            server.ServerErrored += ServerOnServerErrored;
            server.SessionCreated += ServerOnSessionCreated;
        }

        private void ServerOnSessionCreated(object sender, Node e)
        {
            Console.WriteLine($"Session created {e.Id}");
            _ = new NodeEventsHandlers(e);
        }

        private void ServerOnServerErrored(object sender, SocketError e)
        {
            Console.WriteLine($"Server returned error: {e}");
        }
    }
}