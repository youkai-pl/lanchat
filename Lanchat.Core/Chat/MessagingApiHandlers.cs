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

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Message,
            DataTypes.PrivateMessage
        };

        public void Handle(DataTypes type, object data)
        {
            try
            {
                var decryptedMessage = messaging.Encryption.Decrypt((string) data);
                switch (type)
                {
                    case DataTypes.Message:
                        messaging.OnMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                        break;

                    case DataTypes.PrivateMessage:
                        messaging.OnPrivateMessageReceived(decryptedMessage.Truncate(config.MaxMessageLenght));
                        break;
                }
            }
            catch (CryptographicException)
            {
            }
        }
    }
}