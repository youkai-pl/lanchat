﻿using System;
using System.Net.Sockets;
using Lanchat.Core;
using Lanchat.Core.Network;

namespace Lanchat.Probe.Handlers
{
    public class ServerEventsHandlers
    {
        public ServerEventsHandlers(Server server)
        {
            server.ServerErrored += ServerOnServerErrored;
            server.SessionCreated += ServerOnSessionCreated;
        }

        private void ServerOnSessionCreated(object sender, Session e)
        {
            Console.WriteLine($"Session created {e.Id}");
            _ = new EventsHandlers(e);
        }

        private void ServerOnServerErrored(object sender, SocketError e)
        {
            Console.WriteLine($"Server returned error: {e}");
        }
    }
}