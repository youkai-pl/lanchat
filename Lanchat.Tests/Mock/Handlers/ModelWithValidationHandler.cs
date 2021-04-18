using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.Handlers
{
    public class ModelWithValidationHandlerMock : ApiHandler<ModelWithValidation>
    {
        protected override void Handle(ModelWithValidation data)
        { }
    }
}