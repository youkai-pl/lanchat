using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.Node;

namespace Lanchat.Core.ApiHandlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternal node;
        private readonly IOutput output;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        internal HandshakeHandler(
            IPublicKeyEncryption publicKeyEncryption,
            ISymmetricEncryption encryption,
            IOutput output,
            INodeInternal node)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            this.encryption = encryption;
            this.output = output;
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (node.HandshakeReceived)
            {
                return;
            }

            node.Nickname = handshake.Nickname;

            if (!node.IsSession)
            {
                node.SendHandshake();
            }

            try
            {
                publicKeyEncryption.ImportKey(handshake.PublicKey);
                node.Status = handshake.Status;
                output.SendPrivilegedData(encryption.ExportKey());
                node.HandshakeReceived = true;
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}