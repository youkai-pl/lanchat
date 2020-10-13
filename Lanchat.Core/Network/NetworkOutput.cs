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
    public class NetworkOutput
    {
        private readonly Node node;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkOutput(Node node)
        {
            this.node = node;
            serializerOptions = CoreConfig.JsonSerializerOptions;
        }

        /// <summary>
        ///     Send message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendMessage(string content)
        {
            if (!node.Ready)
            {
                return;
            }

            var message = new Message {Content = node.Encryption.Encrypt(content)};
            var data = new Wrapper {Type = DataTypes.Message, Data = message};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        /// <summary>
        ///     Send ping.
        /// </summary>
        public void SendPing()
        {
            if (!node.Ready)
            {
                return;
            }

            var data = new Wrapper {Type = DataTypes.Ping};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = CoreConfig.Nickname,
                PublicKey = node.Encryption.ExportPublicKey()
            };

            var data = new Wrapper {Type = DataTypes.Handshake, Data = handshake};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        internal void SendKey()
        {
            var keyInfo = node.Encryption.ExportAesKey();
            var data = new Wrapper {Type = DataTypes.KeyInfo, Data = keyInfo};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        internal void SendNodesList(IEnumerable<IPAddress> list)
        {
            var stringList = list.Select(x => x.ToString());
            var data = new Wrapper {Type = DataTypes.NodesList, Data = stringList};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}