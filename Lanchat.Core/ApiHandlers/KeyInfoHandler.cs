using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.Node;

namespace Lanchat.Core.ApiHandlers
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