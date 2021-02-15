using Lanchat.Core.Models;

namespace Lanchat.Core.NetworkIO
{
    public interface IApiHandler
    {
        DataTypes DataType { get; }
        void Handle(object data);
    }
}