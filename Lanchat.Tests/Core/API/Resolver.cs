using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Lanchat.Core.API;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.Core.API
{
    public class ResolverTests
    {
        private NodeState nodeState;
        private Resolver resolver;

        [SetUp]
        public void Setup()
        {
            nodeState = new NodeState();
            resolver = new Resolver(nodeState);
            resolver.RegisterHandler(new MessageHandlerMock());
        }

        [Test]
        public void UnknownModel()
        {
            Assert.Catch<ArgumentException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new ModelMock())); });
        }

        [Test]
        public void NoHandler()
        {
            Assert.Catch<ArgumentException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Handshake())); });
        }

        [Test]
        public void NodeNotReady()
        {
            nodeState.Ready = false;
            Assert.Catch<InvalidOperationException>(() => { resolver.HandleJson(NetworkOutput.Serialize(new Message())); });
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