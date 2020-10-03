using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;

namespace Lanchat.Core.Network
{
    public class Client : TcpClient, INetworkElement
    {
        private bool safeDisconnect;
        private int reconnectCounter;

        public Client(string address, int port) : base(address, port)
        {
        }

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<string> DataReceived;
        public event EventHandler<SocketError> SocketErrored;

        public void DisconnectAndStop()
        {
            safeDisconnect = true;
            DisconnectAsync();
            while (IsConnected)
            {
                Thread.Yield();
            }
        }

        public override bool Connect()
        {
            reconnectCounter = 0;
            return base.Connect();
        }

        protected override void OnConnected()
        {
            reconnectCounter = 0;
            Connected?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnDisconnected()
        {
            // If client isn't reconnecting raise event
            if (reconnectCounter == 0)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
            
            // Try reconnect after while
            Thread.Sleep(1000);
            
            // Stop if reconnect counter is greater than 3 or client disconnected safely
            if (safeDisconnect || reconnectCounter > 3)
            {
                return;
            }

            reconnectCounter++;
            ConnectAsync();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            var message = Encoding.UTF8.GetString(buffer, (int) offset, (int) size);
            DataReceived?.Invoke(this, message);
        }

        protected override void OnError(SocketError error)
        {
            SocketErrored?.Invoke(this, error);
        }
    }
}