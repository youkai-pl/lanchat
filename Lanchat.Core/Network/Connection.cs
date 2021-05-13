using System;
using System.Threading.Tasks;

namespace Lanchat.Core.Network
{
    internal class Connection
    {
        private readonly INodeInternal nodeInternal;
        private readonly HandshakeSender handshakeSender;

        public Connection(INodeInternal nodeInternal, HandshakeSender handshakeSender)
        {
            this.nodeInternal = nodeInternal;
            this.handshakeSender = handshakeSender;
            nodeInternal.Host.Disconnected += OnDisconnected;
        }

        internal void Initialize()
        {
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