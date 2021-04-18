using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.Handlers
{
    public class ModelHandlerMock : ApiHandler<Model>
    {
        public bool Handled;

        protected override void Handle(Model data)
        {
            Handled = true;
        }
    }
}