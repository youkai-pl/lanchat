using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;


namespace Lanchat.Core
{
    public class Client : TcpClient
    {
        private readonly Events events;

        public Client(string address, int port, Events events) : base(address, port)
        {
            this.events = events;
        }
        
        public void DisconnectAndStop()
        {
            stop = true;
            DisconnectAsync();
            while (IsConnected)
            {
                Thread.Yield();
            }
        }

        protected override void OnConnected()
        {
            events.OnClientConnected(this);
        }

        protected override void OnDisconnected()
        {
            events.OnClientDisconnected(this);
            
            // Try reconnect after while
            Thread.Sleep(1000);
            if (!stop)
            {
                ConnectAsync();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            events.OnMessageReceived(this,Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
        }

        protected override void OnError(SocketError error)
        {
            events.OnClientError(this, error);
        }

        private bool stop;
    }
}