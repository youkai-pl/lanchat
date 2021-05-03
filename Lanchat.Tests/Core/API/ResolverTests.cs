using System;
using Lanchat.Core.Api;
using Lanchat.Core.Json;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class ResolverTests
    {
        private JsonUtils jsonUtils;
        private NodeMock nodeMock;
        private Resolver resolver;
        private MessageHandlerMock messageHandlerMock;
        private PrivilegedHandler privilegedHandler;
        private Input input;

        [SetUp]
        public void Setup()
        {
            jsonUtils = new JsonUtils();
            nodeMock = new NodeMock();
            resolver = new Resolver(nodeMock);
            input = new Input(resolver);
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
            input.OnDataReceived(this, jsonUtils.Serialize(new Message {Content = "test"}));
            Assert.IsTrue(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidModel()
        {
            input.OnDataReceived(this, jsonUtils.Serialize(new Message {Content = null}));
            Assert.IsFalse(messageHandlerMock.Received);
        }

        [Test]
        public void InvalidOperationExceptionCatch()
        {
            input.OnDataReceived(this, "{}");
        }

        [Test]
        public void JsonExceptionCatch()
        {
            input.OnDataReceived(this, "{\"key\": invalid format}");
        }

        [Test]
        public void ValidationExceptionCatch()
        {
            input.OnDataReceived(this, jsonUtils.Serialize(new ModelWithValidation()));
        }

        [Test]
        public void SingleUseHandler()
        {
            var handler = new SingleUseHandler();
            resolver.RegisterHandler(handler);
            resolver.CallHandler(jsonUtils.Serialize(new Model()));
            resolver.CallHandler(jsonUtils.Serialize(new Model()));
            Assert.AreEqual(1, handler.Counter);
        }
    }
}