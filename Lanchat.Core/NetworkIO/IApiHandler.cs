using System;

namespace Lanchat.Core.NetworkIO
{
    internal interface IApiHandler
    {
        public Type HandledType { get; }
        public void Handle(object data);
    }
}