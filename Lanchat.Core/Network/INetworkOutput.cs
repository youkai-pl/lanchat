using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    internal interface INetworkOutput
    {
        void SendUserData(DataTypes dataType, object content = null);
        void SendSystemData(DataTypes dataType, object content = null);
    }
}