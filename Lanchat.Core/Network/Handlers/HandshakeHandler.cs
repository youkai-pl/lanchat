using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.Network.Handlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternal node;
        private readonly Messaging messaging;
        private readonly HandshakeSender handshakeSender;
        private readonly IOutput output;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        public HandshakeHandler(
            IPublicKeyEncryption publicKeyEncryption,
            ISymmetricEncryption encryption,
            IOutput output,
            INodeInternal node,
            Messaging messaging,
            HandshakeSender handshakeSender)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            this.encryption = encryption;
            this.output = output;
            this.node = node;
            this.messaging = messaging;
            this.handshakeSender = handshakeSender;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            node.Nickname = handshake.Nickname;

            if (!node.IsSession)
            {
                handshakeSender.SendHandshake();
            }

            try
            {
                publicKeyEncryption.ImportKey(handshake.PublicKey);
                messaging.UserStatus = handshake.UserStatus;
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