using Lanchat.Core.Encryption;
using Lanchat.Core.Json;
using Lanchat.Core.Network;
using Lanchat.Core.Tcp;

namespace Lanchat.Core.Api
{
    internal class Output : IOutput
    {
        private readonly IModelEncryption encryption;
        private readonly IHost host;
        private readonly JsonUtils jsonUtils;
        private readonly INodeInternal node;

        public Output(IHost host, INodeInternal node, IModelEncryption encryption)
        {
            this.host = host;
            this.node = node;
            this.encryption = encryption;
            jsonUtils = new JsonUtils();
        }

        public void SendData(object content)
        {
            if (!node.Ready)
            {
                return;
            }

            encryption.EncryptObject(content);
            host.Send(jsonUtils.Serialize(content));
        }

        public void SendPrivilegedData(object content)
        {
            encryption.EncryptObject(content);
            host.Send(jsonUtils.Serialize(content));
        }
    }
}