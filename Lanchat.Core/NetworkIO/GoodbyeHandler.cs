using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core.NetworkIO
{
    public class GoodbyeHandler : IApiHandler
    {
        private readonly INetworkElement networkElement;

        public GoodbyeHandler(INetworkElement networkElement)
        {
            this.networkElement = networkElement;
        }
        
        public DataTypes DataType { get; } = DataTypes.Goodbye;
        public void Handle(object data)
        {
            networkElement.EnableReconnecting = false;
        }
    }
}