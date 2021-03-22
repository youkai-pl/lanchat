using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Tests.Mock
{
    internal class MessageHandlerMock : ApiHandler<Message>
    {
        protected override void Handle(Message data)
        {
        }
    }
}