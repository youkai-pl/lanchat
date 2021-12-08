using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Lanchat.Ipc
{
    public class IpcSocket
    {
        private readonly ManualResetEvent allDone = new(false);
        private readonly string socketPath;
        private readonly List<Socket> handlers = new();

        public IpcSocket(string socketPath)
        {
            this.socketPath = socketPath;
        }

        public void Start()
        {
            var listener = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            var endpoint = new UnixDomainSocketEndPoint(socketPath);
            File.Delete(socketPath);
            listener.Bind(endpoint);
            listener.Listen();

            while (true)
            {
                allDone.Reset();
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                allDone.WaitOne();
            }
        }

        public void Send(string data)
        {
            handlers.ToList().ForEach(x => Send(x, data));
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);
            handlers.Add(handler);
            var state = new StateObject
            {
                Socket = handler
            };
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
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
            Program.Network.Broadcast.SendMessage(content);
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
            try
            {
                handler.EndSend(ar);
            }
            catch (SocketException)
            {
                handlers.Remove(handler);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }
    }
}