using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TcpClient = NetCoreServer.TcpClient;


namespace Lanchat.Core
{
    public class Client : TcpClient
    {
        public Client(string address, int port) : base(address, port) {}
        
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
            Console.WriteLine($"Chat TCP client connected a new session with Id {Id}");
        }

        protected override void OnDisconnected()
        {
            Trace.WriteLine($"Chat TCP client disconnected a session with Id {Id}");

            // Try reconnect after while
            Thread.Sleep(1000);
            if (!stop)
            {
                ConnectAsync();
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            Console.WriteLine(Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP client caught an error with code {error}");
        }

        private bool stop;
    }
}