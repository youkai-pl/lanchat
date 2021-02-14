using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    /// <summary>
    ///     Sending and receiving data using this class.
    /// </summary>
    internal class NetworkOutput : INetworkOutput
    {
        private readonly Node node;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkOutput(Node node)
        {
            this.node = node;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        public void SendData(DataTypes dataType, object content = null)
        {
            if (!node.Ready) return;
            var data = new Wrapper {Type = dataType, Data = content};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        private void SendDataBeforeActivation(DataTypes dataType, object content = null)
        {
            var data = new Wrapper {Type = dataType, Data = content};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = CoreConfig.Nickname,
                Status = CoreConfig.Status,
                PublicKey = node.Encryption.ExportPublicKey()
            };

            SendDataBeforeActivation(DataTypes.Handshake, handshake);
        }

        internal void SendKey()
        {
            var keyInfo = node.Encryption.ExportAesKey();
            SendDataBeforeActivation(DataTypes.KeyInfo, keyInfo);
        }

        internal void SendNodesList(IEnumerable<IPAddress> list)
        {
            var stringList = list.Select(x => x.ToString());
            SendData(DataTypes.NodesList, stringList);
        }

        internal void SendNicknameUpdate(string nickname)
        {
            SendDataBeforeActivation(DataTypes.NicknameUpdate, nickname);
        }

        internal void SendGoodbye()
        {
            SendDataBeforeActivation(DataTypes.Goodbye);
        }

        internal void SendStatusUpdate(Status status)
        {
            SendDataBeforeActivation(DataTypes.StatusUpdate, status);
        }
    }
}