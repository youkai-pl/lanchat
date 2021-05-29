using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;

namespace Lanchat.Core.Chat.Handlers
{
    internal class MessageHandler : ApiHandler<Message>
    {
        private readonly IInternalMessaging messaging;

        public MessageHandler(IInternalMessaging messaging)
        {
            this.messaging = messaging;
        }

        protected override void Handle(Message message)
        {
            if (message.Private)
            {
                messaging.OnPrivateMessageReceived(message.Content);
            }
            else
            {
                messaging.OnMessageReceived(message.Content);
            }
        }
    }
}