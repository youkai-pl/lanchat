using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Tests
{
    public class FakeNetworkOutput : INetworkOutput
    {
        public void SendUserData(DataTypes dataType, object content = null)
        {
        }

        public void SendSystemData(DataTypes dataType, object content = null)
        {
        }
    }
}