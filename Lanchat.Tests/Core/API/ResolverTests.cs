using System;
using Lanchat.Core.API;
using Lanchat.Core.Json;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Encryption;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class ResolverTests
    {
        private JsonUtils jsonUtils;
        private MessageHandlerMock messageHandlerMock;
        private NodeMock nodeMock;
        private PrivilegedHandler privilegedHandler;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            jsonUtils = new JsonUtils();
            nodeMock = new NodeMock();
            resolver = new Resolver(nodeMock, new ModelEncryptionMock());
            messageHandlerMock = new MessageHandlerMock();
            privilegedHandler = new PrivilegedHandler();
            resolver.RegisterHandler(messageHandlerMock);
            resolver.RegisterHandler(privilegedHandler);
        }

        [Test]
        public void UnknownModel()
        {
            Assert.Catch<InvalidOperationException>(() => { resolver.CallHandler(jsonUtils.Serialize(new Model())); });
        }

        [Test]
        public void NoHandler()
        {
            Assert.Catch<InvalidOperationException>(() =>
            {
                resolver.CallHandler(jsonUtils.Serialize(new Handshake()));
            });
        }

        [Test]
        public void NodeNotReady()
        {
            nodeMock.Ready = false;
            resolver.CallHandler(jsonUtils.Serialize(new Message()));
            Assert.IsFalse(messageHandlerMock.Received);
        }

        [Test]
        public void PrivilegedHandler()
        {
            nodeMock.Ready = false;
            resolver.CallHandler(jsonUtils.Serialize(new PrivilegedModel()));
            Assert.IsTrue(privilegedHandler.Received);
        }

        [Test]
        public void ValidModel()
        {
            resolver.OnDataReceived(this, jsonUtils.Serialize(new Message {Content = "test"}));
            Assert.IsTrue(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidModel()
        {
            resolver.OnDataReceived(this, jsonUtils.Serialize(new Message {Content = null}));
            Assert.IsFalse(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidOperationExceptionCatch()
        {
            resolver.OnDataReceived(this, "{}");
        }

        [Test]
        public void JsonExceptionCatch()
        {
            resolver.OnDataReceived(this, "{\"key\": invalid format}");
        }

        [Test]
        public void ValidationExceptionCatch()
        {
            resolver.OnDataReceived(this, jsonUtils.Serialize(new ModelWithValidation()));
        }
    }
}