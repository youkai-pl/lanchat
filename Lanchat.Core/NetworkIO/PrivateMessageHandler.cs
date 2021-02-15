using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class PrivateMessageHandler : IApiHandler
    {
        private readonly Messaging messaging;
        public DataTypes DataType { get; } = DataTypes.PrivateMessage;

        public PrivateMessageHandler(Messaging messaging)
        {
            this.messaging = messaging;
        }
        
        public void Handle(object data)
        {
            var message = (string)data;
            messaging.HandlePrivateMessage(message);
        }
    }
}