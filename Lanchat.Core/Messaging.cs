using System;
using System.Collections.Generic;
using Lanchat.Core.Encryption;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core
{
    public class Messaging : IApiHandler
    {
        private readonly IStringEncryption encryption;
        private readonly INetworkOutput networkOutput;

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
            networkOutput.SendUserData(DataTypes.Message, encryption.Encrypt(content));
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            networkOutput.SendUserData(DataTypes.PrivateMessage, encryption.Encrypt(content));
        }
        
        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Message, 
            DataTypes.PrivateMessage
        };

        public void Handle(DataTypes type, string data)
        {
            if (type == DataTypes.Message)
            {
                var decryptedMessage = encryption.Decrypt(data);
                if (decryptedMessage == null) return;
                MessageReceived?.Invoke(this, decryptedMessage.Truncate(CoreConfig.MaxMessageLenght));
                return;
            }

            if (type == DataTypes.PrivateMessage)
            {
                var decryptedPrivateMessage = encryption.Decrypt(data);
                if (decryptedPrivateMessage == null) return;
                PrivateMessageReceived?.Invoke(this,
                    decryptedPrivateMessage.Truncate(CoreConfig.MaxMessageLenght));
            }
        }
    }
}