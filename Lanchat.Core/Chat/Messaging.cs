using System;
using System.ComponentModel;
using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;

namespace Lanchat.Core.Chat
{
    internal class Messaging : IMessaging, IInternalMessaging
    {
        private readonly IOutput output;
        private UserStatus userStatus;

        public Messaging(IOutput output)
        {
            this.output = output;
        }

        public void OnMessageReceived(string e)
        {
            MessageReceived?.Invoke(this, e);
        }

        public void OnPrivateMessageReceived(string e)
        {
            PrivateMessageReceived?.Invoke(this, e);
        }

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

        public event EventHandler<string> MessageReceived;
        public event EventHandler<string> PrivateMessageReceived;
        public event PropertyChangedEventHandler PropertyChanged;

        public void SendMessage(string content)
        {
            output.SendData(new Message {Content = content});
        }

        public void SendPrivateMessage(string content)
        {
            output.SendData(new Message
            {
                Content = content,
                Private = true
            });
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}