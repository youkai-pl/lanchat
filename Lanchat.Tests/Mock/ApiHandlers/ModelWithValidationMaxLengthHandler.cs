using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.ApiHandlers
{
    public class ModelWithValidationMaxLengthHandler : ApiHandler<ModelWithValidationMaxLength>
    {
        public bool Received { get; private set; }

        protected override void Handle(ModelWithValidationMaxLength data)
        {
            Received = true;
        }
    }
}