using Lanchat.Core.API;

namespace Lanchat.Tests.Mock
{
    public class ModelWithValidationHandlerMock : ApiHandler<ModelWithValidationMock>
    {
        protected override void Handle(ModelWithValidationMock data)
        {
        }
    }
}