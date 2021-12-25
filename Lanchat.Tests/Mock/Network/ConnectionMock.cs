using Lanchat.Core.Network;

namespace Lanchat.Tests.Mock.Network
{
    public class ConnectionMock : IConnection
    {
        public bool HandshakeReceived { get; set; } = true;

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public void SendHandshake()
        {
            throw new System.NotImplementedException();
        }
    }
}