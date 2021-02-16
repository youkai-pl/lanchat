using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Message,
            DataTypes.PrivateMessage
        };
        
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
        
        public void Handle(DataTypes type, object data)
        {
            try
            {
                var decryptedMessage = encryption.Decrypt((string)data);
                switch (type)
                {
                    case DataTypes.Message:
                        MessageReceived?.Invoke(this, decryptedMessage.Truncate(CoreConfig.MaxMessageLenght));
                        break;
                
                    case DataTypes.PrivateMessage:
                        PrivateMessageReceived?.Invoke(this, decryptedMessage.Truncate(CoreConfig.MaxMessageLenght));
                        break;
                }
            }
            catch (CryptographicException)
            { }
        }
    }
}