using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternal node;

        internal KeyInfoHandler(ISymmetricEncryption encryption, INodeInternal node)
        {
            this.encryption = encryption;
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            if (node.Ready)
            {
                return;
            }

            if (!node.HandshakeReceived)
            {
                return;
            }

            if (keyInfo == null)
            {
                return;
            }

            try
            {
                encryption.ImportKey(keyInfo);
                node.Ready = true;
                node.OnConnected();
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
        }
    }
}