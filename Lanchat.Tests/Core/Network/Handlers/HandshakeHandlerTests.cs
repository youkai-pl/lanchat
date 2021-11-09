using Lanchat.Core.Encryption;
using Lanchat.Core.Encryption.Models;
using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Core.Network.Handlers;
using Lanchat.Core.Network.Models;
using Lanchat.Tests.Mock.Api;
using Lanchat.Tests.Mock.Config;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Network;
using Lanchat.Tests.Mock.Tcp;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Network.Handlers
{
    public class HandshakeHandlerTests
    {
        private HandshakeHandler handshakeHandler;
        private NodeAesMock nodeAesMock;
        private NodeMock nodeMock;
        private NodeRsa nodeRsa;
        private OutputMock outputMock;

        [SetUp]
        public void Setup()
        {
            nodeRsa = new NodeRsa(new NodesDatabaseMock(), new LocalRsa(new NodesDatabaseMock()));
            nodeAesMock = new NodeAesMock();
            outputMock = new OutputMock();
            nodeMock = new NodeMock(outputMock);

            handshakeHandler = new HandshakeHandler(
                nodeRsa,
                nodeAesMock,
                outputMock,
                nodeMock,
                new HostMock(),
                new User(nodeMock),
                new Connection(nodeMock, new HostMock(), outputMock, new ConfigMock(), nodeRsa));
        }

        [Test]
        public void ValidHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = "test",
                UserStatus = UserStatus.Online,
                PublicKey = nodeRsa.ExportKey()
            };
            handshakeHandler.Handle(handshake);

            nodeRsa.Encrypt(new byte[] { 0x10 });
            Assert.IsTrue(handshakeHandler.Disabled);
            Assert.AreEqual(handshake.UserStatus, nodeMock.User.UserStatus);
            Assert.NotNull(outputMock.LastOutput);
        }

        [Test]
        public void InvalidKey()
        {
            var eventRaised = false;
            nodeMock.CannotConnect += (_, _) => { eventRaised = true; };

            var handshake = new Handshake
            {
                Nickname = "test",
                UserStatus = UserStatus.Online,
                PublicKey = new PublicKey()
            };
            handshakeHandler.Handle(handshake);
            Assert.IsTrue(eventRaised);
        }
    }
}