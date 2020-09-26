using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Session : TcpSession
    {
        private readonly Events events;

        public Session(TcpServer server, Events events) : base(server)
        {
            this.events = events;
        }

        protected override void OnConnected()
        {
            events.OnClientConnected(this);
        }

        protected override void OnDisconnected()
        {
            events.OnClientDisconnected(this);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            events.OnMessageReceived(this, message);
        }

        protected override void OnError(SocketError error)
        {
            events.OnServerError(this, error);
        }
    }
}