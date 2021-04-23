using Lanchat.Core.Api;
using Lanchat.Tests.Mock.Models;

namespace Lanchat.Tests.Mock.Handlers
{
    public class SingleUseHandler : ApiHandler<Model>
    {
        public int Counter { get; private set; }
        protected override void Handle(Model data)
        {
            Counter++;
            Disable = true;
        }
    }
}