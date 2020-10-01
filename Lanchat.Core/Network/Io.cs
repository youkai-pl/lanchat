using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    public class Io
    {
        private readonly INetworkElement networkElement;

        public Io(INetworkElement networkElement)
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