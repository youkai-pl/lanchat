using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Identity;
using Lanchat.Core.Network.Models;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Network.Handlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly HandshakeSender handshakeSender;
        private readonly IHost host;
        private readonly IInternalUser user;
        private readonly INodeInternal node;
        private readonly IOutput output;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        public HandshakeHandler(
            IPublicKeyEncryption publicKeyEncryption,
            ISymmetricEncryption encryption,
            IOutput output,
            INodeInternal node,
            IHost host,
            IInternalUser user,
            HandshakeSender handshakeSender)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            this.encryption = encryption;
            this.output = output;
            this.node = node;
            this.host = host;
            this.user = user;
            this.handshakeSender = handshakeSender;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (!host.IsSession)
            {
                handshakeSender.SendHandshake();
            }

            try
            {
                publicKeyEncryption.ImportKey(handshake.PublicKey);
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