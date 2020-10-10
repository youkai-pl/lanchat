using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class NetworkIO
    {
        private readonly Node node;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkIO(Node node)
        {
            this.node = node;

            // Treat enums like a string
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }

        // Input
        internal Wrapper DeserializeInput(string json)
        {
            return JsonSerializer.Deserialize<Wrapper>(json, serializerOptions);
        }

        // Output
        public void SendMessage(string content)
        {
            if (!node.Ready)
            {
                return;
            }

            var message = new Message {Content = content};
            var data = new Wrapper {Type = DataTypes.Message, Data = message};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

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
            var handshake = new Handshake {Nickname = Config.Nickname};
            var data = new Wrapper {Type = DataTypes.Handshake, Data = handshake};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }

        internal void SendNewNodeInfo(IPAddress ipAddress)
        {
            var ip = ipAddress.ToString();
            var data = new Wrapper {Type = DataTypes.NewNode, Data = ip};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}