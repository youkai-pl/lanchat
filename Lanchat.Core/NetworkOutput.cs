using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    public class NetworkOutput
    {
        private readonly INetworkElement networkElement;

        public NetworkOutput(INetworkElement networkElement)
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