using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Lanchat.Core.API;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Handlers;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class ResolverTests
    {
        private NodeState nodeState;
        private Resolver resolver;
        private PrivilegedHandler privilegedHandler;

        [SetUp]
        public void Setup()
        {
            nodeState = new NodeState();
            resolver = new Resolver(nodeState);
            privilegedHandler = new PrivilegedHandler();
            resolver.RegisterHandler(new MessageHandlerMock());
            resolver.RegisterHandler(privilegedHandler);
        }

        [Test]
        public void UnknownModel()
        {
            Assert.Catch<InvalidOperationException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Model())); });
        }

        [Test]
        public void NoHandler()
        {
            Assert.Catch<InvalidOperationException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Handshake())); });
        }

        [Test]
        public void NodeNotReady()
        {
            nodeState.Ready = false;
            Assert.Catch<InvalidOperationException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Message())); });
        }
        
        [Test]
        public void PrivilegedHandler()
        {
            nodeState.Ready = false;
            resolver.HandleJson(NetworkOutput.Serialize(new PrivilegedModel()));
            Assert.IsTrue(privilegedHandler.Received);
        }

        [Test]
        public void InvalidModel()
        {
            Assert.Catch<ValidationException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Message())); });
        }

        [Test]
        public void InvalidJson()
        {
            Assert.Catch<JsonException>(() => { resolver.HandleJson("not a json"); });
        }
    }
}