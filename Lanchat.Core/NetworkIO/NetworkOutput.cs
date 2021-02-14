using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
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

        public void SendUserData(DataTypes dataType, object content = null)
        {
            if (!node.Ready) return;
            SendSystemData(dataType, content);
        }

        public void SendSystemData(DataTypes dataType, object content = null)
        {
            var data = new Wrapper {Type = dataType, Data = content};
            node.NetworkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}