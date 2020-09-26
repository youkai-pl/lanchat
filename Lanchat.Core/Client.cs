using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;

namespace Lanchat.Core
{
    public class Client : TcpClient
    {
        private bool stop;

        public event EventHandler ClientConnected;
        public event EventHandler ClientDisconnected;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<SocketError> ClientErrored; 

        public Client(string address, int port) : base(address, port)
        {
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
            ClientConnected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            ClientDisconnected?.Invoke(this, EventArgs.Empty);
            
            // Try reconnect after while
            Thread.Sleep(1000);
            if (!stop)
            {
                ConnectAsync();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            MessageReceived?.Invoke(this, message);
        }

        protected override void OnError(SocketError error)
        {
            ClientErrored?.Invoke(this, error);
        }
    }
}