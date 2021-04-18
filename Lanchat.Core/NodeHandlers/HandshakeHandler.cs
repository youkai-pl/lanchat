using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternals nodeInternals;
        private readonly IOutput output;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        internal HandshakeHandler(
            IPublicKeyEncryption publicKeyEncryption,
            ISymmetricEncryption encryption,
            IOutput output,
            INodeInternals nodeInternals)
        {
            this.publicKeyEncryption = publicKeyEncryption;
            this.encryption = encryption;
            this.output = output;
            this.nodeInternals = nodeInternals;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (nodeInternals.HandshakeReceived)
            {
                return;
            }

            nodeInternals.Nickname = handshake.Nickname;

            if (!nodeInternals.IsSession)
            {
                nodeInternals.SendHandshake();
            }

            try
            {
                publicKeyEncryption.ImportKey(handshake.PublicKey);
                nodeInternals.Status = handshake.Status;
                output.SendPrivilegedData(encryption.ExportKey());
                nodeInternals.HandshakeReceived = true;
            }
            catch (CryptographicException)
            {
                nodeInternals.OnCannotConnect();
            }
        }
    }
}