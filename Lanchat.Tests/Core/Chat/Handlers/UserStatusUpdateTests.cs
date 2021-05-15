using Lanchat.Core.Chat;
using Lanchat.Core.Chat.Handlers;
using Lanchat.Core.Chat.Models;
using Lanchat.Tests.Mock.Network;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Chat.Handlers
{
    public class USerStatusUpdateTests
    {
        private NodeMock nodeMock;
        private UserStatusUpdateHandler userStatusUpdateHandler;

        [SetUp]
        public void Setup()
        {
            nodeMock = new NodeMock();
            userStatusUpdateHandler = new UserStatusUpdateHandler(nodeMock.Messaging);
        }

        [Test]
        public void NewStatus()
        {
            var statusUpdate = new UserStatusUpdate
            {
                NewUserStatus = UserStatus.AwayFromKeyboard
            };

            userStatusUpdateHandler.Handle(statusUpdate);
            Assert.AreEqual(statusUpdate.NewUserStatus, nodeMock.Messaging.UserStatus);
        }
    }
}