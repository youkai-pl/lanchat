using Lanchat.Core.API;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class NetworkInputTests
    {
        private NetworkInput networkInput;
        private Resolver resolver;
        private ModelHandlerMock modelHandlerMock;

        [SetUp]
        public void Setup()
        {
            modelHandlerMock = new ModelHandlerMock();
            resolver = new Resolver(new NodeState());
            resolver.RegisterHandler(modelHandlerMock);
            networkInput = new NetworkInput(resolver);
        }

        [Test]
        public void JsonSplit()
        {
            var fullString = NetworkOutput.Serialize(new ModelMock());
            var firstPart = fullString.Substring(0, 10);
            var secondPart = fullString.Substring(10);
            networkInput.ProcessReceivedData(null, firstPart);
            networkInput.ProcessReceivedData(null, secondPart);
            Assert.True(modelHandlerMock.Handled);
        }
    }
}