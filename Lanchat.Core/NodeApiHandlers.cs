using System;
using System.Collections.Generic;
using System.Net;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    internal class NodeApiHandlers : IApiHandler
    {
        private readonly Node node;

        internal NodeApiHandlers(Node node)
        {
            this.node = node;
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Goodbye,
            DataTypes.KeyInfo,
            DataTypes.NodesList,
            DataTypes.StatusUpdate,
            DataTypes.Handshake,
            DataTypes.NicknameUpdate
        };

        public void Handle(DataTypes type, object data)
        {
            switch (type)
            {
                case DataTypes.Goodbye:
                    node.NetworkElement.EnableReconnecting = false;
                    break;
                
                case DataTypes.KeyInfo:
                {
                    var keyInfo = (KeyInfo) data;
                    if (keyInfo == null) return;

                    node.Encryptor.ImportAesKey(keyInfo);
                    node.Ready = true;
                    node.OnConnected();
                    break;
                }
                
                case DataTypes.NodesList:
                {
                    var stringList = (List<string>)data;
                    var list = new List<IPAddress>();

                    // Convert strings to ip addresses.
                    stringList?.ForEach(x =>
                    {
                        if (IPAddress.TryParse(x, out var ipAddress)) list.Add(ipAddress);
                    });

                    node.OnNodesListReceived(list);
                    break;
                }
                
                case DataTypes.Handshake:
                {
                    var handshake = (Handshake)data;
                    if (handshake == null) return;

                    node.Nickname = handshake.Nickname.Truncate(CoreConfig.MaxNicknameLenght);
                    node.Encryptor.ImportPublicKey(handshake.PublicKey);
                    node.Status = handshake.Status;
                    node.NetworkOutput.SendSystemData(DataTypes.KeyInfo, node.Encryptor.ExportAesKey());
                    break;
                }
                
                case DataTypes.StatusUpdate:
                {
                    if (Enum.TryParse<Status>(data.ToString(), out var newStatus)) node.Status = newStatus;
                    break;
                }
               
                case DataTypes.NicknameUpdate:
                {
                    var newNickname = data.ToString();
                    node.Nickname = newNickname.Truncate(CoreConfig.MaxNicknameLenght);
                    break;
                }
            }
        }
    }
}