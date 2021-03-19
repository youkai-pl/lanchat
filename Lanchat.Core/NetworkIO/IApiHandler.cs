using System;
using System.Collections.Generic;

namespace Lanchat.Core.NetworkIO
{
    internal interface IApiHandler
    {
        IEnumerable<Type> HandledDataTypes { get; }
        void Handle(Type type, object data = null);
    }
}