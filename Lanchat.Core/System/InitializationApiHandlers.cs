using System.Collections.Generic;
using System.Diagnostics;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.System
{
    internal class InitializationApiHandlers : IApiHandler
    {
        private readonly Node node;
        private bool handshakeReceived;

        internal InitializationApiHandlers(Node node)
        {
            this.node = node;
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Handshake,
            DataTypes.KeyInfo
        };

        public void Handle(DataTypes type, object data)
        {
            switch (type)
            {
                case DataTypes.Handshake:
                {
                    if (handshakeReceived && !node.UnderReconnecting) return;

                    var handshake = (Handshake) data;
                    if (handshake == null) return;

                    node.Nickname = handshake.Nickname.Truncate(CoreConfig.MaxNicknameLenght);

                    if (!node.NetworkElement.IsSession)
                    {
                        node.SendHandshakeAndWait();
                    }

                    try
                    {
                        node.Encryptor.ImportPublicKey(handshake.PublicKey);
                        node.Status = handshake.Status;
                        node.NetworkOutput.SendSystemData(DataTypes.KeyInfo, node.Encryptor.ExportAesKey());
                        handshakeReceived = true;
                    }
                    catch (InvalidKeyImportException)
                    {
                        node.Dispose();
                    }

                    break;
                }

                case DataTypes.KeyInfo:
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

                    break;
                }
            }
        }
    }
}