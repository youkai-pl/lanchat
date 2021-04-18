using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.NodeHandlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly ISymmetricEncryption encryption;
        private readonly INodeInternals nodeInternals;

        internal KeyInfoHandler(ISymmetricEncryption encryption, INodeInternals nodeInternals)
        {
            this.encryption = encryption;
            this.nodeInternals = nodeInternals;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            if (nodeInternals.Ready)
            {
                return;
            }

            if (!nodeInternals.HandshakeReceived)
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
                nodeInternals.Ready = true;
                nodeInternals.OnConnected();
            }
            catch (CryptographicException)
            {
                nodeInternals.OnCannotConnect();
            }
        }
    }
}