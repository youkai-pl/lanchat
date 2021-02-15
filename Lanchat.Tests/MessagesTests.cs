using Lanchat.Core;
using Lanchat.Core.Encryption;
using Lanchat.Core.NetworkIO;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class MessagesSendTests
    {
        private NetworkOutput networkOutput;
        private NetworkInput networkInput;
        private NetworkMock networkMock;
        private Messaging messaging;
        private Encryptor encryptor;

        [SetUp]
        public void Setup()
        {
            CoreConfig.MaxMessageLenght = 5;
            var nodeState = new NodeState();

            encryptor = new Encryptor();
            encryptor.ImportPublicKey(encryptor.ExportPublicKey());
            encryptor.ImportAesKey(encryptor.ExportAesKey());
            networkMock = new NetworkMock();
            networkOutput = new NetworkOutput(networkMock, nodeState);
            messaging = new Messaging(networkOutput, encryptor);
            networkInput = new NetworkInput(nodeState);
            networkInput.ApiHandlers.Add(messaging);
        }

        [Test]
        public void ValidMessageSend()
        {
            var testMessage = "test";
            var receivedMessage = string.Empty;
            
            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void ValidPrivateMessageSend()
        {
            var testMessage = "test";
            var receivedMessage = string.Empty;
            
            messaging.PrivateMessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);

            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void TooLongMessageSend()
        {
            var testMessage = "1234567890";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);
            
            messaging.SendMessage(testMessage);
            Assert.AreEqual("12345...", receivedMessage);
        }

        [Test]
        public void TooLongPrivateMessageSend()
        {
            var testMessage = "1234567890";
            var receivedMessage = string.Empty;

            messaging.PrivateMessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);
            
            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual("12345...", receivedMessage);
        }
    }
}