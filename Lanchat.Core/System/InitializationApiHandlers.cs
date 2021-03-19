using System;
using System.Collections.Generic;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.System
{
    internal class InitializationApiHandlers : IApiHandler
    {
        private readonly Node node;
        private readonly IConfig config;
        private bool handshakeReceived;

        internal InitializationApiHandlers(Node node, IConfig config)
        {
            this.node = node;
            this.config = config;
        }

        public IEnumerable<Type> HandledDataTypes { get; } = new[]
        {
            typeof(Handshake),
            typeof(KeyInfo)
        };

        public void Handle(Type type, object data)
        {
            
                if(type == typeof(Handshake))
                {
                    if (handshakeReceived && !node.UnderReconnecting) return;

                    var handshake = (Handshake) data;
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
                        handshakeReceived = true;
                    }
                    catch (InvalidKeyImportException)
                    {
                        node.Dispose();
                    }
                }

                else if (type == typeof(KeyInfo))
                {
                    if (node.Ready) return;
                    if (!handshakeReceived) return;

                    var keyInfo = (KeyInfo) data;
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
}