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
        private InternalNodeRsa internalNodeRsa;
        private NodeAesMock nodeAesMock;
        private NodeMock nodeMock;
        private OutputMock outputMock;

        [SetUp]
        public void Setup()
        {
            internalNodeRsa = new InternalNodeRsa(new RsaDatabaseMock(), new LocalPublicKey(new RsaDatabaseMock()));
            nodeAesMock = new NodeAesMock();
            outputMock = new OutputMock();
            nodeMock = new NodeMock(outputMock);

            handshakeHandler = new HandshakeHandler(
                internalNodeRsa,
                nodeAesMock,
                outputMock,
                nodeMock,
                new HostMock(),
                new User(nodeMock),
                new Connection(nodeMock, new HostMock(), outputMock, new ConfigMock(), internalNodeRsa));
        }

        [Test]
        public void ValidHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = "test",
                UserStatus = UserStatus.Online,
                PublicKey = internalNodeRsa.ExportKey()
            };
            handshakeHandler.Handle(handshake);

            internalNodeRsa.Encrypt(new byte[] {0x10});
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