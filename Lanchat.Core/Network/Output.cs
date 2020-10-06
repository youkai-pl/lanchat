using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Output
    {
        private readonly Node node;
        public readonly JsonSerializerOptions SerializerOptions;

        internal Output(Node node)
        {
            this.node = node;
            
            // Treat enums like a string
            SerializerOptions = new JsonSerializerOptions{
                Converters ={
                    new JsonStringEnumConverter()
                }
            };
        }

        internal void SendMessage(string content)
        {
            if (!node.Ready)
            {
                return;
            }
            
            var message = new Message {Content = content};
            var data = new Wrapper {Type = DataTypes.Message, Data = message};
            node.SendAsync(JsonSerializer.Serialize(data, SerializerOptions));
        }

        internal void SendPing()
        {
            if (!node.Ready)
            {
                return;
            }
            
            var data = new Wrapper {Type = DataTypes.Ping};
            node.SendAsync(JsonSerializer.Serialize(data, SerializerOptions));
        }

        internal void SendHandshake()
        {
            var handshake = new Handshake {Nickname = Config.Nickname};
            var data = new Wrapper {Type = DataTypes.Handshake, Data = handshake};
            node.SendAsync(JsonSerializer.Serialize(data, SerializerOptions));
        }
    }
}