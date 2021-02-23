using System;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.Chat
{
    public class Messaging
    {
        internal readonly IStringEncryption Encryption;
        private readonly INetworkOutput networkOutput;

        internal Messaging(INetworkOutput networkOutput, IStringEncryption encryption)
        {
            this.networkOutput = networkOutput;
            Encryption = encryption;
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
            networkOutput.SendUserData(DataTypes.Message, Encryption.Encrypt(content));
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            networkOutput.SendUserData(DataTypes.PrivateMessage, Encryption.Encrypt(content));
        }

        internal void OnMessageReceived(string e)
        {
            MessageReceived?.Invoke(this, e);
        }

        internal void OnPrivateMessageReceived(string e)
        {
            PrivateMessageReceived?.Invoke(this, e);
        }
    }
}