using System;

namespace Lanchat.Core.NetworkIO
{
    public abstract class ApiHandler<T> : IApiHandler
    {
        public Type HandledType { get; } = typeof(T);
        protected abstract void Handle(T data);

        public void Handle(object data)
        {
            Handle((T)data);
        }
    }
}