using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        private readonly Events events;
        
        public Server(IPAddress address, int port, Events events) : base(address, port)
        {
            this.events = events;
        }

        protected override TcpSession CreateSession() { return new Session(this); }

        protected override void OnError(SocketError error)
        {
           events.OnServerError(error);
        }
    }
}