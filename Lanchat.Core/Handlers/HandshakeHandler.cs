using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Handlers
{
    internal class HandshakeHandler : ApiHandler<Handshake>
    {
        private readonly Node node;
        private readonly IConfig config;

        internal HandshakeHandler(Node node, IConfig config)
        {
            this.node = node;
            this.config = config;
        }

        protected override void Handle(Handshake handshake)
        {
            if (node.HandshakeReceived && !node.UnderReconnecting) return;

            if (handshake == null) return;

            node.Nickname = handshake.Nickname.Truncate(config.MaxNicknameLenght);

            if (!node.NetworkElement.IsSession)
            {
                node.SendHandshakeAndWait();
            }

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