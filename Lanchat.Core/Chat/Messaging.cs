using System;
using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;

namespace Lanchat.Core.Chat
{
    internal class Messaging : IMessaging, IInternalMessaging
    {
        private readonly IOutput output;

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

        public event EventHandler<string> MessageReceived;
        public event EventHandler<string> PrivateMessageReceived;

        public void SendMessage(string content)
        {
            output.SendData(new Message { Content = content });
        }

        public void SendPrivateMessage(string content)
        {
            output.SendData(new Message
            {
                Content = content,
                Private = true
            });
        }
    }
}