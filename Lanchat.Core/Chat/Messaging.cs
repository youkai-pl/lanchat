using System;
using System.ComponentModel;
using Lanchat.Core.Api;
using Lanchat.Core.Models;

namespace Lanchat.Core.Chat
{
    /// <summary>
    ///     Basic chat features.
    /// </summary>
    public class Messaging : INotifyPropertyChanged
    {
        private readonly IOutput output;
        private UserStatus userStatus;

        internal Messaging(IOutput output)
        {
            this.output = output;
        }

        /// <summary>
        ///     <see cref="Lanchat.Core.Chat.UserStatus"/>
        /// </summary>
        public UserStatus UserStatus
        {
            get => userStatus;
            set
            {
                if (userStatus == value)
                {
                    return;
                }
                
                userStatus = value;
                OnPropertyChanged(nameof(UserStatus));
            }
        }

        /// <summary>
        ///     Message received.
        /// </summary>
        public event EventHandler<string> MessageReceived;

        /// <summary>
        ///     Private message received.
        /// </summary>
        public event EventHandler<string> PrivateMessageReceived;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Send message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendMessage(string content)
        {
            output.SendData(new Message {Content = content});
        }

        /// <summary>
        ///     Send private message.
        /// </summary>
        /// <param name="content">Message content.</param>
        public void SendPrivateMessage(string content)
        {
            output.SendData(new Message
            {
                Content = content,
                Private = true
            });
        }

        internal void OnMessageReceived(string e)
        {
            MessageReceived?.Invoke(this, e);
        }

        internal void OnPrivateMessageReceived(string e)
        {
            PrivateMessageReceived?.Invoke(this, e);
        }
        
        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}