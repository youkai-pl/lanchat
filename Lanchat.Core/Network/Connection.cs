using System;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal class Connection : IConnection
    {
        private readonly IConfig config;
        private readonly IHost host;
        private readonly IInternalNodeRsa internalNodeRsa;
        private readonly INodeInternal nodeInternal;
        private readonly IOutput output;
        private bool cannotConnectHandled;

        public Connection(
            INodeInternal nodeInternal,
            IHost host,
            IOutput output,
            IConfig config,
            IInternalNodeRsa internalNodeRsa)
        {
            this.nodeInternal = nodeInternal;
            this.host = host;
            this.output = output;
            this.config = config;
            this.internalNodeRsa = internalNodeRsa;
            host.Disconnected += OnDisconnected;
        }

        public bool HandshakeReceived { get; set; }

        public void Initialize()
        {
            if (host.IsSession)
            {
                SendHandshake();
            }

            Task.Delay(5000).ContinueWith(_ =>
            {
                if (nodeInternal.Ready || cannotConnectHandled)
                {
                    return;
                }

                cannotConnectHandled = true;
                nodeInternal.OnCannotConnect();
            });
        }

        public void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                UserStatus = config.UserStatus,
                PublicKey = internalNodeRsa.ExportKey()
            };

            output.SendPrivilegedData(handshake);
        }

        private void OnDisconnected(object sender, EventArgs _)
        {
            if (nodeInternal.Ready)
            {
                nodeInternal.OnDisconnected();
            }
            else if (!cannotConnectHandled)
            {
                cannotConnectHandled = true;
                nodeInternal.OnCannotConnect();
            }
        }
    }
}