using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Lanchat.Ipc
{
    public class SocketListener
    {
        private readonly ManualResetEvent allDone = new(false);
        private readonly string socketPath;

        public SocketListener(string socketPath)
        {
            this.socketPath = socketPath;
        }

        public void StartListening()
        {
            var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            var endpoint = new UnixDomainSocketEndPoint(socketPath);
            File.Delete(socketPath);
            listener.Bind(endpoint);
            listener.Listen();

            while (true)
            {
                allDone.Reset();
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                allDone.WaitOne();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);
            var state = new StateObject
            {
                Socket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            var content = string.Empty;
            var state = (StateObject)ar.AsyncState;
            var handler = state.Socket;
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead == 0)
            {
                return;
            }

            state.StringBuilder.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead));
            content = state.StringBuilder.ToString();
            Console.WriteLine(content);
            state.StringBuilder.Clear();
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void Send(Socket handler, string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;
            handler.EndSend(ar);
        }
    }
}