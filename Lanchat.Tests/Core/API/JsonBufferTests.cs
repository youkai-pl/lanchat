using Lanchat.Core.Api;
using Lanchat.Core.Json;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class NetworkInputTests
    {
        private JsonBuffer jsonBuffer;
        private JsonUtils jsonUtils;
        private ModelHandlerMock modelHandlerMock;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            modelHandlerMock = new ModelHandlerMock();
            resolver = new Resolver(new NodeMock(), new ModelEncryptionMock());
            resolver.RegisterHandler(modelHandlerMock);
            resolver.RegisterHandler(new ModelWithValidationHandlerMock());
            jsonBuffer = new JsonBuffer();
            jsonUtils = new JsonUtils();
        }

        [Test]
        public void JsonSplit()
        {
            var fullString = jsonUtils.Serialize(new Model());
            var firstPart = fullString[..10];
            var secondPart = fullString[10..];
            jsonBuffer.AddToBuffer(firstPart);
            jsonBuffer.ReadBuffer().ForEach(resolver.CallHandler);
            jsonBuffer.AddToBuffer(secondPart);
            jsonBuffer.ReadBuffer().ForEach(resolver.CallHandler);
            Assert.True(modelHandlerMock.Handled);
        }
    }
}