using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;
using Lanchat.Core.Network;
using Lanchat.Core.System;

namespace Lanchat.Core.NetworkIO
{
    /// <summary>
    ///     Sending and receiving data using this class.
    /// </summary>
    internal class NetworkOutput : INetworkOutput
    {
        private readonly INetworkElement networkElement;
        private readonly INodeState nodeState;
        private readonly JsonSerializerOptions serializerOptions;

        internal NetworkOutput(INetworkElement networkElement, INodeState nodeState)
        {
            this.networkElement = networkElement;
            this.nodeState = nodeState;
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }

        public void SendUserData(DataTypes dataType, object content = null)
        {
            if (!nodeState.Ready) return;
            SendSystemData(dataType, content);
        }

        public void SendSystemData(DataTypes dataType, object content = null)
        {
            var data = new Dictionary<string, string> {{dataType.ToString(), JsonSerializer.Serialize(content, serializerOptions)}};
            networkElement.SendAsync(JsonSerializer.Serialize(data, serializerOptions));
        }
    }
}