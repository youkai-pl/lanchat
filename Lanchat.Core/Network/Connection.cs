using System;
using System.Threading.Tasks;
using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network
{
    internal class Connection
    {
        private readonly IOutput output;
        private readonly IConfig config;
        private readonly INodePublicKey nodePublicKey;
        private readonly IHost host;
        private readonly INodeInternal nodeInternal;

        public Connection(
            INodeInternal nodeInternal,
            IHost host,
            IOutput output, 
            IConfig config, 
            INodePublicKey nodePublicKey)
        {
            this.nodeInternal = nodeInternal;
            this.host = host;
            this.output = output;
            this.config = config;
            this.nodePublicKey = nodePublicKey;
            host.Disconnected += OnDisconnected;
        }

        internal void Initialize()
        {
            if (host.IsSession)
            {
                SendHandshake();
            }

            Task.Delay(5000).ContinueWith(_ =>
            {
                if (!nodeInternal.Ready)
                {
                    nodeInternal.OnCannotConnect();
                }
            });
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                UserStatus = config.UserStatus,
                PublicKey = nodePublicKey.ExportKey()
            };

            output.SendPrivilegedData(handshake);
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