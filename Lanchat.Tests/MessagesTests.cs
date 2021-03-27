using System.ComponentModel.DataAnnotations;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class MessagesSendTests
    {
        private Encryptor encryptor;
        private Messaging messaging;
        private NetworkInput networkInput;
        private NetworkMock networkMock;
        private NetworkOutput networkOutput;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            var nodeState = new NodeState();
            encryptor = new Encryptor();
            encryptor.ImportPublicKey(encryptor.ExportPublicKey());
            encryptor.ImportAesKey(encryptor.ExportAesKey());
            networkMock = new NetworkMock();
            networkOutput = new NetworkOutput(networkMock, nodeState);
            messaging = new Messaging(networkOutput, encryptor);
            resolver = new Resolver(nodeState);
            resolver.Handlers.Add(new MessageHandler(messaging));
            resolver.Models.Add(typeof(Message));
            networkInput = new NetworkInput(resolver);
        }

        [Test]
        public void ValidMessageSend()
        {
            var testMessage = new string('a', 20);
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void TooLongMessageSend()
        {
            var testMessage = new string('a', 2000);

            Assert.Catch<ValidationException>(() =>
            {
                networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);
                messaging.SendMessage(testMessage);
            });
        }


        [Test]
        public void WeirdText()
        {
            var testMessage =
                "ẗ̴̝̱̦̝͉͉̬̩̙́̎e̷̡̧̡̢̮̩͓̯̞̼̖̜̥̭̣̙͕̲̳̰̱̾̈͗̉̈́͐́̿̿̕ş̵̡̣̣̳̺̘̲̦͕̣̹̯̰̘̟̰͕̗̰̦͍̩̩̱̩͖̖͍̈́̊͆̾̀̄̾͐̈̈̍̃̔̉̋̐̔͒̒̍̎̇̏͌̑̚͜t̴͙̭̠͇̹̫͇̗̥̗͍̀̒̈́́͑̈́̃͌̽̈́̏̈̉͘̕̚͜͝ͅͅ";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (sender, s) => networkInput.ProcessReceivedData(sender, s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }
    }
}