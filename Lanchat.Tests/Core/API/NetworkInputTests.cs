using Lanchat.Core.API;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
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
            resolver.RegisterHandler(new ModelWithValidationHandlerMock());
            networkInput = new NetworkInput(resolver);
        }

        [Test]
        public void JsonSplit()
        {
            var fullString = NetworkOutput.Serialize(new Model());
            var firstPart = fullString.Substring(0, 10);
            var secondPart = fullString.Substring(10);
            networkInput.ProcessReceivedData(null, firstPart);
            networkInput.ProcessReceivedData(null, secondPart);
            Assert.True(modelHandlerMock.Handled);
        }
        
        [Test]
        public void InvalidOperationExceptionCatch()
        {
            networkInput.ProcessReceivedData(null, "{}");
        }
        
        [Test]
        public void JsonExceptionCatch()
        {
            networkInput.ProcessReceivedData(null, "{\"key\": invalid format}");
        }
        
        [Test]
        public void ArgumentExceptionCatch()
        {
            networkInput.ProcessReceivedData(null, "{\"null\": \"null\"}");
        }
        
        [Test]
        public void ArgumentNullExceptionCatch()
        {
            networkInput.ProcessReceivedData(null, "{\"Model\": \"null\"}");
        }
        
        [Test]
        public void ValidationExceptionCatch()
        {
            networkInput.ProcessReceivedData(null, NetworkOutput.Serialize(new ModelWithValidation()));
        }
    }
}