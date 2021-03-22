using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.NodeHandlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly Node node;

        internal KeyInfoHandler(Node node)
        {
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            if (node.Ready) return;
            if (!node.HandshakeReceived) return;

            if (keyInfo == null) return;

            try
            {
                node.Encryptor.ImportAesKey(keyInfo);
                node.Ready = true;
                node.OnConnected();
            }
            catch (InvalidKeyImportException)
            {
                node.Dispose();
            }
        }
    }
}