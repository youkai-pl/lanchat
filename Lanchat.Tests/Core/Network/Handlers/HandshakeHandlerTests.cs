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
        private NodeMock nodeMock;
        private OutputMock outputMock;
        private PublicKeyEncryption publicKeyEncryption;
        private SymmetricEncryptionMock symmetricEncryptionMock;

        [SetUp]
        public void Setup()
        {
            publicKeyEncryption = new PublicKeyEncryption();
            symmetricEncryptionMock = new SymmetricEncryptionMock();
            outputMock = new OutputMock();
            nodeMock = new NodeMock(outputMock);

            handshakeHandler = new HandshakeHandler(
                publicKeyEncryption,
                symmetricEncryptionMock,
                outputMock,
                nodeMock,
                new HostMock(),
                new User(nodeMock),
                new Connection(nodeMock, new HostMock(), outputMock, new ConfigMock(), publicKeyEncryption));
        }

        [Test]
        public void ValidHandshake()
        {
            var handshake = new Handshake
            {
                Nickname = "test",
                UserStatus = UserStatus.Online,
                PublicKey = publicKeyEncryption.ExportKey()
            };
            handshakeHandler.Handle(handshake);

            publicKeyEncryption.Encrypt(new byte[] {0x10});
            Assert.IsTrue(handshakeHandler.Disabled);
            Assert.AreEqual(handshake.UserStatus, nodeMock.User.UserStatus);
            Assert.NotNull(outputMock.LastOutput);
        }

        [Test]
        public void InvalidKey()
        {
            var eventRaised = false;
            nodeMock.CannotConnect += (_, _) =>
            {
                eventRaised = true;
            };
            
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