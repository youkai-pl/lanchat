using System.Text.Json;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class HandshakeHandler : IApiHandler
    {
        private readonly Node node;

        public HandshakeHandler(Node node)
        {
            this.node = node;
        }
        
        public DataTypes DataType { get; } = DataTypes.Handshake;
        public void Handle(object data)
        {
            var handshake = JsonSerializer.Deserialize<Handshake>((string)data, CoreConfig.JsonSerializerOptions);
            if (handshake == null)
            {
                return;
            }
            
            node.Nickname = handshake.Nickname.Truncate(CoreConfig.MaxNicknameLenght);
            node.Encryptor.ImportPublicKey(handshake.PublicKey);
            node.Status = handshake.Status;
            node.NetworkOutput.SendSystemData(DataTypes.KeyInfo, node.Encryptor.ExportAesKey());
        }
    }
}