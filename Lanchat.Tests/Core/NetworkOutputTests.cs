using Lanchat.Core.API;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core
{
    public class NetworkOutputTests
    {
        private NetworkMock networkMock;
        private NodeState nodeState;
        private Output output;

        [SetUp]
        public void Setup()
        {
            networkMock = new NetworkMock();
            nodeState = new NodeState();
            output = new Output(networkMock, nodeState, new ModelEncryptionMock());
        }

        [Test]
        public void SendToReadyNode()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            output.SendData(new Model());
            Assert.IsTrue(dataReceived);
        }

        [Test]
        public void SendToNotReadyNode()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeState.Ready = false;
            output.SendData(new Model());
            Assert.IsFalse(dataReceived);
        }

        [Test]
        public void SendPrivilegedData()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeState.Ready = false;
            output.SendPrivilegedData(new Model());
            Assert.IsTrue(dataReceived);
        }
    }
}