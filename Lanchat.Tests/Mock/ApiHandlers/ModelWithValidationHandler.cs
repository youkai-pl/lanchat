using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    public class ModelWithValidationHandlerRequired : ApiHandler<ModelWithValidationRequired>
    {
        protected override void Handle(ModelWithValidationRequired data)
        { }
    }
}