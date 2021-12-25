using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Identity;
using Lanchat.Core.Network.Models;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Network.Handlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly IConnection connection;
        private readonly INodeAes encryption;
        private readonly IHost host;
        private readonly IInternalNodeRsa internalNodeRsa;
        private readonly INodeInternal node;
        private readonly IOutput output;
        private readonly IInternalUser user;

        public HandshakeHandler(
            IInternalNodeRsa internalNodeRsa,
            INodeAes encryption,
            IOutput output,
            INodeInternal node,
            IHost host,
            IInternalUser user,
            IConnection connection)
        {
            this.internalNodeRsa = internalNodeRsa;
            this.encryption = encryption;
            this.output = output;
            this.node = node;
            this.host = host;
            this.user = user;
            this.connection = connection;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (!host.IsSession)
            {
                connection.SendHandshake();
            }

            try
            {
                internalNodeRsa.ImportKey(handshake.PublicKey, host.Endpoint.Address);
                user.Nickname = handshake.Nickname;
                user.UserStatus = handshake.UserStatus;
                output.SendPrivilegedData(encryption.ExportKey());
                Disabled = true;
                connection.HandshakeReceived = true;
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}