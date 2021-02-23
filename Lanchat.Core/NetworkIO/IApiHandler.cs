using System.Collections.Generic;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    internal interface IApiHandler
    {
        IEnumerable<DataTypes> HandledDataTypes { get; }
        void Handle(DataTypes type, object data = null);
    }
}