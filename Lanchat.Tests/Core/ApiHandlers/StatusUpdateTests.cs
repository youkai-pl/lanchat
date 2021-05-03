using Lanchat.Core.ApiHandlers;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.Core.ApiHandlers
{
    public class StatusUpdateTests
    {
        private StatusUpdateHandler statusUpdateHandler;
        private NodeMock nodeMock;
        
        [SetUp]
        public void Setup()
        {
            nodeMock = new NodeMock();
            statusUpdateHandler = new StatusUpdateHandler(nodeMock);
        }

        [Test]
        public void NewStatus()
        {
            var statusUpdate = new StatusUpdate
            {
                NewStatus = Status.AwayFromKeyboard
            };
            
            statusUpdateHandler.Handle(statusUpdate);
            Assert.AreEqual(statusUpdate.NewStatus, nodeMock.Status);
        }
    }
}