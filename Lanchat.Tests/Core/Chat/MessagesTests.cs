using Lanchat.Core.Api;
using Lanchat.Core.Chat;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Encryption;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Chat
{
    public class MessagesSendTests
    {
        private MessageHandler messageHandler;
        private Messaging messaging;
        private NetworkMock networkMock;
        private Output output;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            var nodeState = new NodeMock();
            networkMock = new NetworkMock();
            output = new Output(networkMock, nodeState);
            messaging = new Messaging(output);
            messageHandler = new MessageHandler(messaging);
            resolver = new Resolver(nodeState);
            resolver.RegisterHandler(messageHandler);
        }

        [Test]
        public void ValidMessageSend()
        {
            const string testMessage = "test message";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void PrivateMessageSend()
        {
            const string testMessage = "test message";
            var receivedMessage = string.Empty;

            messaging.PrivateMessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void WeirdText()
        {
            const string testMessage =
                "ẗ̴̝̱̦̝͉͉̬̩̙́̎e̷̡̧̡̢̮̩͓̯̞̼̖̜̥̭̣̙͕̲̳̰̱̾̈͗̉̈́͐́̿̿̕ş̵̡̣̣̳̺̘̲̦͕̣̹̯̰̘̟̰͕̗̰̦͍̩̩̱̩͖̖͍̈́̊͆̾̀̄̾͐̈̈̍̃̔̉̋̐̔͒̒̍̎̇̏͌̑̚͜t̴͙̭̠͇̹̫͇̗̥̗͍̀̒̈́́͑̈́̃͌̽̈́̏̈̉͘̕̚͜͝ͅͅ";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }
    }
}