using System;
using System.Net.Sockets;

namespace Lanchat.Core
{
    public sealed class Events
    {
        public event EventHandler<SocketError> ServerError;
        public event EventHandler<string> TestEvent; 
        
        internal void OnServerError(SocketError e)
        {
            ServerError?.Invoke(this, e);
        }

        internal void OnTestEvent(string e)
        {
            TestEvent?.Invoke(this, e);
        }
    }
}