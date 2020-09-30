using System;
using System.Net.Sockets;
using Lanchat.Core.Network;

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
            Console.WriteLine("Session error");
            Console.WriteLine($"{session.Id} / {e}");
            Console.WriteLine("");
        }

        private void SessionOnSessionDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client disconnected");
            Console.WriteLine($"{session.Id} / disposed endpoint");
            Console.WriteLine("");
        }

        private void SessionOnSessionConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client connected");
            Console.WriteLine($"{session.Id} / {session.Socket.RemoteEndPoint}");
            Console.WriteLine("");
        }

        private void SessionOnMessageReceived(object sender, string e)
        {
            Console.WriteLine("Message received");
            Console.WriteLine($"{session.Id} / {session.Socket.RemoteEndPoint}");
            Console.WriteLine(e);
            Console.WriteLine("");
        }
    }
}