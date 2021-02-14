using Lanchat.Core;
using Lanchat.Core.Encryption;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class MessagesSendTests
    {
        private FakeNetworkOutput fakeNetworkOutput;
        private Messaging messaging;
        private Encryptor encryptor;

        [SetUp]
        public void Setup()
        {
            CoreConfig.MaxMessageLenght = 5;
            encryptor = new Encryptor();
            encryptor.ImportPublicKey(encryptor.ExportPublicKey());
            encryptor.ImportAesKey(encryptor.ExportAesKey());
            fakeNetworkOutput = new FakeNetworkOutput(encryptor);
            messaging = new Messaging(fakeNetworkOutput, encryptor);
        }

        [Test]
        public void ValidMessageSend()
        {
            var testMessage = "test";
            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, fakeNetworkOutput.ReceivedMessage);
        }
        
        [Test]
        public void ValidPrivateMessageSend()
        {
            var testMessage = "test";
            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual(testMessage, fakeNetworkOutput.ReceivedMessage);
        }
        
        [Test]
        public void TooLongMessageSend()
        {
            var testMessage = "1234567890";
            messaging.SendMessage(testMessage);
            Assert.AreEqual("12345...", fakeNetworkOutput.ReceivedMessage);
        }
        
        [Test]
        public void TooLongPrivateMessageSend()
        {
            var testMessage = "1234567890";
            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual("12345...", fakeNetworkOutput.ReceivedMessage);
        }
    }
}