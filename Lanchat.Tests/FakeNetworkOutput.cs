using Lanchat.Core;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Tests
{
    public class FakeNetworkOutput : INetworkOutput
    {
        private readonly IStringEncryption encryption;

        public FakeNetworkOutput(IStringEncryption encryption)
        {
            this.encryption = encryption;
        }
        
        public string ReceivedMessage { get; private set; }
        public void SendUserData(DataTypes dataType, object content = null)
        {
            ReceivedMessage = encryption.Decrypt((string)content).Truncate(CoreConfig.MaxMessageLenght);
        }

        public void SendSystemData(DataTypes dataType, object content = null)
        {
        }
    }
}