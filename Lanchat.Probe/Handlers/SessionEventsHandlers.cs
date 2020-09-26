using System;
using System.Net.Sockets;
using Lanchat.Core;

namespace Lanchat.Probe.Handlers
{
    public class SessionEventsHandlers
    {
        private readonly Session session;
        
        public SessionEventsHandlers(Session session)
        {
            this.session = session;
            session.MessageReceived += SessionOnMessageReceived;
            session.SessionConnected += SessionOnSessionConnected;
            session.SessionDisconnected += SessionOnSessionDisconnected;
            session.SessionErrored += SessionOnSessionErrored;
        }

        private void SessionOnSessionErrored(object sender, SocketError e)
        {
            Console.WriteLine($"Session returned error: {e}");
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Session disconnected");
        }

        private void SessionOnSessionConnected(object sender, EventArgs e)
        {
            Console.WriteLine($"Session connected");
        }

        private void SessionOnMessageReceived(object sender, string e)
        {
            Console.WriteLine($"Message received: {e}");
        }
    }
}