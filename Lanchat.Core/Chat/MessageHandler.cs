using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    internal class MessageHandler : ApiHandler<Message>
    {
        private readonly Messaging messaging;

        internal MessageHandler(Messaging messaging)
        {
            this.messaging = messaging;
        }

        protected override void Handle(Message message)
        {
            try
            {
                if (message.Private)
                {
                    var decryptedMessage = messaging.Encryption.DecryptString(message.Content);
                    messaging.OnPrivateMessageReceived(decryptedMessage);
                }
                else
                {
                    var decryptedMessage = messaging.Encryption.DecryptString(message.Content);
                    messaging.OnMessageReceived(decryptedMessage);
                }
            }
            catch (CryptographicException)
            {
            }
        }
    }
}