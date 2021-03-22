using System;

namespace Lanchat.Core.NetworkIO
{
    internal interface IApiHandler
    {
        public Type HandledType { get; }
        public bool Privileged { get; }
        public void Handle(object data);
    }
}