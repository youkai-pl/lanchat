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
        private readonly ISymmetricEncryption encryption;
        private readonly IHost host;
        private readonly INodeInternal node;
        private readonly IOutput output;
        private readonly IPublicKeyEncryption publicKeyEncryption;
        private readonly IInternalUser user;
        private readonly Connection connection;

        public HandshakeHandler(
            IPublicKeyEncryption publicKeyEncryption,
            ISymmetricEncryption encryption,
            IOutput output,
            INodeInternal node,
            IHost host,
            IInternalUser user,
            Connection connection)
        {
            this.publicKeyEncryption = publicKeyEncryption;
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
                publicKeyEncryption.ImportKey(handshake.PublicKey, host.Endpoint.Address);
                user.Nickname = handshake.Nickname;
                user.UserStatus = handshake.UserStatus;
                output.SendPrivilegedData(encryption.ExportKey());
                Disabled = true;
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}