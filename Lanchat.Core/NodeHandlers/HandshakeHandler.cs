using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.NodeHandlers
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

            if (!node.NetworkElement.IsSession) node.SendHandshakeAndWait();

            try
            {
                node.Encryptor.ImportPublicKey(handshake.PublicKey);
                node.Status = handshake.Status;
                node.NetworkOutput.SendSystemData(node.Encryptor.ExportAesKey());
                node.HandshakeReceived = true;
            }
            catch (InvalidKeyImportException)
            {
                node.Dispose();
            }
        }
    }
}