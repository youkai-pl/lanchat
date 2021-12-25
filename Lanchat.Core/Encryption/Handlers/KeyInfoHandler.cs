using System;
using System.Diagnostics;
using System.Security.Cryptography;
using Lanchat.Core.Api;
using Lanchat.Core.Encryption.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.Encryption.Handlers
{
    internal class KeyInfoHandler : ApiHandler<KeyInfo>
    {
        private readonly INodeAes encryption;
        private readonly INodeInternal node;

        public KeyInfoHandler(INodeAes encryption, INodeInternal node)
        {
            this.encryption = encryption;
            this.node = node;
            Privileged = true;
        }

        protected override void Handle(KeyInfo keyInfo)
        {
            if(!node.Connection.HandshakeReceived)
            {
                Trace.WriteLine("Cannot handle KeyInfo before handshake");
                return;
            }

            try
            {
                encryption.ImportKey(keyInfo);
                node.Ready = true;
                Disabled = true;
                node.OnConnected();
            }
            catch (CryptographicException)
            {
                node.OnCannotConnect();
            }
            catch (ArgumentNullException)
            {
                node.OnCannotConnect();
            }
        }
    }
}