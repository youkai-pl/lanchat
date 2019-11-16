using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lanchat.Common.TcpLib
{
    public class Host
    {
        public void Start(int port)
        {
            // Start server
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();

            // Wait for connection
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                while (client.Connected)
                {
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
                    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    OnTcpEvent(new
                    {
                        type = "message",
                        content = dataReceived
                    }, EventArgs.Empty);
                }
            }
        }

        // Input event
        public delegate void TcpEventHandler(object o, EventArgs e);

        public event TcpEventHandler TcpEvent;

        protected virtual void OnTcpEvent(object o, EventArgs e)
        {
            TcpEvent(o, EventArgs.Empty);
        }
    }

    public class Client
    {
        private TcpClient tcpclnt;
        private NetworkStream nwStream;

        public void Connect(string ip, int port)
        {
            tcpclnt = new TcpClient(ip, port);
            nwStream = tcpclnt.GetStream();

            OnTcpEvent(new
            {
                type = "connected"
            }, EventArgs.Empty);
        }

        public void Send(string content)
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(content);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        // Input event
        public delegate void TcpEventHandler(object o, EventArgs e);

        public event TcpEventHandler TcpEvent;

        protected virtual void OnTcpEvent(object o, EventArgs e)
        {
            TcpEvent(o, EventArgs.Empty);
        }
    }
}