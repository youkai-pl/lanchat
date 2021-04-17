using System;
using System.Security.Cryptography;
using Lanchat.Core.API;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    internal class MessageHandler : ApiHandler<Message>
    {
        private readonly Messaging messaging;
        private readonly SymmetricEncryption symmetricEncryption;

        internal MessageHandler(Messaging messaging, SymmetricEncryption symmetricEncryption)
        {
            this.messaging = messaging;
            this.symmetricEncryption = symmetricEncryption;
        }

        protected override void Handle(Message message)
        {
            try
            {
                DecryptMessageAndRaiseEvent(message);
            }
            catch (CryptographicException)
            {
            }
            catch (FormatException)
            {
            }
        }

        private void DecryptMessageAndRaiseEvent(Message message)
        {
            var decryptedMessage = symmetricEncryption.DecryptString(message.Content);

            if (message.Private)
                messaging.OnPrivateMessageReceived(decryptedMessage);
            else
                messaging.OnMessageReceived(decryptedMessage);
        }
    }
}