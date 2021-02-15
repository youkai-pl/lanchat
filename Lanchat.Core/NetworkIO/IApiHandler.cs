using System.Collections.Generic;
using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public interface IApiHandler
    {
        IEnumerable<DataTypes> HandledDataTypes { get; }
        void Handle(DataTypes type, string data);
    }
}