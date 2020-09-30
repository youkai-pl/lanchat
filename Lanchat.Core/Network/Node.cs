using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Node
    {
        private readonly INetworkElement networkElement;

        public Node(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
        }

        public void SendMessage(string content)
        {
            var data = new DataWrapper<Message>(new Message {Content = content});
            var json = JsonSerializer.Serialize(data);
            networkElement.SendAsync(json);
        }
    }
}