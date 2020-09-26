using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe.Handlers
{
    public class ServerEventsHandlers
    {
        private readonly Server server;
        
        public ServerEventsHandlers(Server server)
        {
            this.server = server;
            server.ServerErrored += ServerOnServerErrored;
            server.SessionCreated += ServerOnSessionCreated;
        }

        private void ServerOnSessionCreated(object sender, Session e)
        {
            Console.WriteLine($"Session created {e.Id}");
            _ = new SessionEventsHandlers(e);
        }

        private void ServerOnServerErrored(object sender, SocketError e)
        {
            Console.WriteLine($"Server returned error: {e}");
        }
    }
}