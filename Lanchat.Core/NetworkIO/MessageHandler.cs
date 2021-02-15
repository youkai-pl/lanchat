using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public class MessageHandler : IApiHandler
    {
        private readonly Messaging messaging;
        public DataTypes DataType { get; } = DataTypes.Message;

        public MessageHandler(Messaging messaging)
        {
            this.messaging = messaging;
        }
        
        public void Handle(object data)
        {
            var message = (string)data;
            messaging.HandleMessage(message);
        }
    }
}