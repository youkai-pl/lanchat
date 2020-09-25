using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Session : TcpSession
    {
        public Session(TcpServer server) : base(server) {}

        protected override void OnConnected()
        {
            Trace.WriteLine($"Chat TCP session with Id {Id} connected!");
        }

        protected override void OnDisconnected()
        {
            Trace.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Trace.WriteLine("Incoming: " + message);
        }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }
}