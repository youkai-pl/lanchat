using System;
using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Output
    {
        private readonly Node node;

        internal Output(Node node)
        {
            this.node = node;
        }

        internal void SendMessage(string content)
        {
            var message = new Message {Content = content};
            var data = new Wrapper {Type = DataTypes.Message, Data = message};
            node.SendAsync(JsonSerializer.Serialize(data));
        }

        internal void SendPing()
        {
            var data = new Wrapper {Type = DataTypes.Ping};
            node.SendAsync(JsonSerializer.Serialize(data));
        }

        internal void SendHandshake(string nickname)
        {
            var handshake = new Handshake {Nickname = nickname};
            var data = new Wrapper {Type = DataTypes.Handshake, Data = handshake};
            node.SendAsync(JsonSerializer.Serialize(data));
        }
    }
}