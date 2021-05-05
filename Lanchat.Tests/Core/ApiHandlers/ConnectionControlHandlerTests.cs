using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class ConnectionControlHandlerTests
    {
        private ConnectionControlHandler connectionControlHandler;
        private NetworkMock networkElement;
        
        [SetUp]
        public void Setup()
        {
            networkElement = new NetworkMock();
            connectionControlHandler = new ConnectionControlHandler(networkElement);
        }

        [Test]
        public void RemoteClose()
        {
            var data = new ConnectionControl
            {
                Status = ConnectionStatus.RemoteDisconnect
            };
            connectionControlHandler.Handle(data);
            Assert.IsTrue(networkElement.Closed);
        }
    }
}