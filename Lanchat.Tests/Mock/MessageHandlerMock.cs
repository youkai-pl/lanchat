using Lanchat.Core.API;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock
{
    internal class MessageHandlerMock : ApiHandler<Message>
    {
        protected override void Handle(Message data)
        {
        }
    }
}