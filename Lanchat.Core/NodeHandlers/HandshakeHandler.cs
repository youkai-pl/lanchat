using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly Node node;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        internal HandshakeHandler(IPublicKeyEncryption publicKeyEncryption, ISymmetricEncryption encryption, Node node)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            this.encryption = encryption;
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
                node.Output.SendPrivilegedData(encryption.ExportKey());
                node.HandshakeReceived = true;
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}