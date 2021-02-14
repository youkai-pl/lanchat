using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.Network;

namespace Lanchat.Core
{
    public class Messaging
    {
        private readonly INetworkOutput networkOutput;
        private readonly IStringEncryption encryption;

        internal Messaging(INetworkOutput networkOutput, IStringEncryption encryption)
        {
            this.networkOutput = networkOutput;
            this.encryption = encryption;
        }

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;
        
        /// <summary>
        ///     Private message received.
        /// </summary>
        public event EventHandler<string> PrivateMessageReceived;
        
        /// <summary>
        ///     Send message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendMessage(string content)
        {
            networkOutput.SendData(DataTypes.Message, encryption.Encrypt(content));
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            networkOutput.SendData(DataTypes.PrivateMessage, encryption.Encrypt(content));
        }

        internal void HandleMessage(string content)
        {
            var decryptedMessage = encryption.Decrypt(content);
            if (decryptedMessage == null) return;
            MessageReceived?.Invoke(this, decryptedMessage.Truncate(CoreConfig.MaxMessageLenght)); 
        }

        internal void HandlePrivateMessage(string content)
        {
            var decryptedPrivateMessage = encryption.Decrypt(content);
            if (decryptedPrivateMessage == null) return;
            PrivateMessageReceived?.Invoke(this,
                decryptedPrivateMessage.Truncate(CoreConfig.MaxMessageLenght));
        }
    }
}