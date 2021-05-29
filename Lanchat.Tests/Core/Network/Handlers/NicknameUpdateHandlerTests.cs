using Lanchat.Core.Identity.Handlers;
using Lanchat.Core.Identity.Models;
using Lanchat.Tests.Mock.Identity;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Network.Handlers
{
    public class NicknameUpdateHandlerTests

    {
        private NicknameUpdateHandler nicknameUpdateHandler;
        private UserMock userMock;

        [SetUp]
        public void Setup()
        {
            userMock = new UserMock();
            nicknameUpdateHandler = new NicknameUpdateHandler(userMock);
        }

        [Test]
        public void NewNickname()
        {
            var nicknameUpdate = new NicknameUpdate
            {
                NewNickname = "new-nickname"
            };

            nicknameUpdateHandler.Handle(nicknameUpdate);
            Assert.AreEqual(nicknameUpdate.NewNickname, userMock.Nickname);
        }
    }
}