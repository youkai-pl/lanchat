using System;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network
{
    internal class Connection
    {
        private readonly INodeInternal nodeInternal;
        private readonly IInput input;
        private readonly IHost host;
        private readonly HandshakeSender handshakeSender;

        public Connection(
            INodeInternal nodeInternal,
            IInput input,
            IHost host,
            HandshakeSender handshakeSender)
        {
            this.nodeInternal = nodeInternal;
            this.input = input;
            this.host = host;
            this.handshakeSender = handshakeSender;
            nodeInternal.Host.Disconnected += OnDisconnected;
        }

        internal void Initialize()
        {
            host.DataReceived += input.OnDataReceived;
            
            if (nodeInternal.IsSession)
            {
                handshakeSender.SendHandshake();
            }

            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!nodeInternal.Ready)
                {
                    nodeInternal.OnCannotConnect();
                }
            });
        }

        private void OnDisconnected(object sender, EventArgs _)
        {
            if (nodeInternal.Ready)
            {
                nodeInternal.OnDisconnected();
            }
            else
            {
                nodeInternal.OnCannotConnect();
            }
        }
    }
}