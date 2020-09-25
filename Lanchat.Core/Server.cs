using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace Lanchat.Core
{
    public class Server : TcpServer
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession() { return new Session(this); }

        protected override void OnError(SocketError error)
        {
            Trace.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}