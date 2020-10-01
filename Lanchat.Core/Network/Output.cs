using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Output
    {
        private readonly INode node;

        public Output(INode node)
        {
            this.node = node;
        }

        public void SendMessage(string content)
        {
            var data = new DataWrapper<Message>(new Message {Content = content});
            var json = JsonSerializer.Serialize(data);
            node.SendAsync(json);
        }
    }
}