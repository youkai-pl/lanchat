using System.ComponentModel.DataAnnotations;
using Lanchat.Core.API;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class MessagesSendTests
    {
        private PublicKeyEncryption publicKeyEncryption;
        private SymmetricEncryption symmetricEncryption;
        private Messaging messaging;
        private NetworkInput networkInput;
        private NetworkMock networkMock;
        private NetworkOutput networkOutput;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            var nodeState = new NodeState();
            publicKeyEncryption = new PublicKeyEncryption();
            symmetricEncryption = new SymmetricEncryption(publicKeyEncryption);
            publicKeyEncryption.ImportKey(publicKeyEncryption.ExportKey());
            symmetricEncryption.ImportKey(symmetricEncryption.ExportKey());
            networkMock = new NetworkMock();
            networkOutput = new NetworkOutput(networkMock, nodeState);
            messaging = new Messaging(networkOutput, symmetricEncryption);
            resolver = new Resolver(nodeState);
            resolver.RegisterHandler(new MessageHandler(messaging));
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