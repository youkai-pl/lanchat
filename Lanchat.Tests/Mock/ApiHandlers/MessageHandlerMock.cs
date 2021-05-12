using Lanchat.Core.Api;
using Lanchat.Core.Chat.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    internal class MessageHandlerMock : ApiHandler<Message>
    {
        public bool Received;

        protected override void Handle(Message data)
        {
            Received = true;
        }
    }
}