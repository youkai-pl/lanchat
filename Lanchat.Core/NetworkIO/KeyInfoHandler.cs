using System.Text.Json;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class KeyInfoHandler : IApiHandler
    {
        private readonly Node node;

        public KeyInfoHandler(Node node)
        {
            this.node = node;
        }
        
        public DataTypes DataType { get; } = DataTypes.KeyInfo;
        public void Handle(object data)
        {
            var keyInfo = JsonSerializer.Deserialize<KeyInfo>((string)data, CoreConfig.JsonSerializerOptions);
            if (keyInfo == null)
            {
                return;
            }
            
            node.Encryptor.ImportAesKey(keyInfo);
            node.Ready = true;
            node.OnConnected();
        }
    }
}