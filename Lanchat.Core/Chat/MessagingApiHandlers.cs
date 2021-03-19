using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Chat
{
    internal class MessagingApiHandlers : IApiHandler
    {
        private readonly Messaging messaging;
        private readonly IConfig config;

        internal MessagingApiHandlers(Messaging messaging, IConfig config)
        {
            this.config = config;
            this.messaging = messaging;
        }

        public IEnumerable<Type> HandledDataTypes { get; } = new[]
        {
            typeof(Message),
            typeof(PrivateMessage)
        };

        public void Handle(Type type, object data)
        {
            try
            {
                if (type == typeof(Message))
                {
                    var message = (Message) data;
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                }
                else if (type == typeof(PrivateMessage))
                {
                    var message = (PrivateMessage) data;
                    var decryptedMessage = messaging.Encryption.Decrypt(message.Content);
                    messaging.OnPrivateMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                }
            }
            catch (CryptographicException)
            {
            }
        }
    }
}