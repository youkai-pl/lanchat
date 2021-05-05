using Lanchat.Core.Api;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Api
{
    public class OutputTests
    {
        private NetworkMock networkMock;
        private NodeMock nodeMock;
        private Output output;

        [SetUp]
        public void Setup()
        {
            networkMock = new NetworkMock();
            nodeMock = new NodeMock
            {
                Ready = true
            };
            output = new Output(networkMock, nodeMock);
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
            nodeMock.Ready = false;
            output.SendData(new Model());
            Assert.IsFalse(dataReceived);
        }

        [Test]
        public void SendPrivilegedData()
        {
            var dataReceived = false;
            networkMock.DataReceived += (_, _) => { dataReceived = true; };
            nodeMock.Ready = false;
            output.SendPrivilegedData(new Model());
            Assert.IsTrue(dataReceived);
        }
    }
}