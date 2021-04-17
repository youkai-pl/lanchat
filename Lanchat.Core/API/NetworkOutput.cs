using System;
using System.Linq;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Json;
using Lanchat.Core.Network;
using Lanchat.Core.NodeHandlers;

namespace Lanchat.Core.API
{
    /// <summary>
    ///     Send data other of type not belonging to standard Lanchat.Core set.
    /// </summary>
    public class NetworkOutput
    {
        private readonly INetworkElement networkElement;
        private readonly INodeState nodeState;
        private readonly JsonUtils jsonUtils;
        private readonly SymmetricEncryption encryption;

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState, SymmetricEncryption encryption)
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
            this.encryption = encryption;
            jsonUtils = new JsonUtils();
        }

        /// <summary>
        ///     Send data.
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendData(object content)
        {
            if (!nodeState.Ready) return;
            EncryptPropertiesWithAttribute(content);
            networkElement.Send(jsonUtils.Serialize(content));
        }
        
        /// <summary>
        ///     Send the data before marking the node as ready (Handshake, KeyInfo...).
        /// </summary>
        /// <param name="content">Object to send.</param>
        public void SendPrivilegedData(object content)
        {
            EncryptPropertiesWithAttribute(content);
            networkElement.Send(jsonUtils.Serialize(content));
        }
        
        private void EncryptPropertiesWithAttribute(object content)
        {
            var props = content
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(EncryptAttribute)));

            props.ForEach(x =>
            {
                var value = x.GetValue(content)?.ToString();
                x.SetValue(content, encryption.EncryptString(value), null);
            });
        }
    }
}