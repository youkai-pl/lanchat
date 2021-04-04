using Lanchat.Core.API;

namespace Lanchat.Tests.Mock
{
    public class ModelHandlerMock : ApiHandler<ModelMock>
    {
        public bool Handled;
        protected override void Handle(ModelMock data)
        {
            Handled = true;
        }
    }
}