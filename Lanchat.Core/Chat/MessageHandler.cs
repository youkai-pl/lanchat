using System.Security.Cryptography;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

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
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnPrivateMessageReceived(decryptedMessage);
                }
                else
                {
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnMessageReceived(decryptedMessage);
                }
            }
            catch (CryptographicException)
            {
            }
        }
    }
}