using Lanchat.Core.Api;
using Lanchat.Core.Config;
using Lanchat.Core.Encryption;
using Lanchat.Core.Network.Models;

namespace Lanchat.Core.Network
{
    internal class HandshakeSender
    {
        private readonly IOutput output;
        private readonly IConfig config;
        private readonly IPublicKeyEncryption publicKeyEncryption;

        public HandshakeSender(IOutput output, IConfig config, IPublicKeyEncryption publicKeyEncryption)
        {
            this.output = output;
            this.config = config;
            this.publicKeyEncryption = publicKeyEncryption;
        }
        
        internal void SendHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = config.Nickname,
                UserStatus = config.UserStatus,
                PublicKey = publicKeyEncryption.ExportKey()
            };

            output.SendPrivilegedData(handshake);
        }
    }
}