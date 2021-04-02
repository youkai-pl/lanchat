using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.P2P.NodeHandlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly Node node;

        internal HandshakeHandler(Node node)
        {
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(Handshake handshake)
        {
            if (node.HandshakeReceived) return;
            node.Nickname = handshake.Nickname;

            if (!node.IsSession) node.SendHandshake();

            try
            {
                node.Encryptor.ImportPublicKey(handshake.PublicKey);
                node.Status = handshake.Status;
                node.NetworkOutput.SendPrivilegedData(node.Encryptor.ExportAesKey());
                node.HandshakeReceived = true;
            }
            catch (InvalidKeyImportException)
            {
                node.Dispose();
            }
        }
    }
}