using Lanchat.Core.Models;

namespace Lanchat.Core.Network
{
    internal interface INetworkOutput
    {
        void SendData(DataTypes dataType, object content = null);
    }
}