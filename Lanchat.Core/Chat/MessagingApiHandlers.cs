using System;
using System.Security.Cryptography;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Chat
{
    internal class MessagingApiHandlers : ApiHandler<Message>
    {
        private readonly Messaging messaging;
        private readonly IConfig config;

        internal MessagingApiHandlers(Messaging messaging, IConfig config)
        {
            this.config = config;
            this.messaging = messaging;
        }

        protected override void Handle(Message message)
        {
            try
            {
                if (message.Private)
                {
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnPrivateMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                }
                else
                {
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                }
            }
            catch (CryptographicException)
            {
            }
        }
    }
}