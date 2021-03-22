using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests
{
    public class ResolverTests
    {
        private Resolver resolver;
        private NodeState nodeState;

        [SetUp]
        public void Setup()
        {
            nodeState = new NodeState();
            resolver = new Resolver(nodeState);
            resolver.Models.Add(typeof(Message));
            resolver.Models.Add(typeof(Handshake));
            resolver.Handlers.Add(new MessageHandlerMock());
        }

        [Test]
        public void UnknownModel()
        {
            var data = new Dictionary<string, object> {{"test", null}};
            Assert.Catch<ArgumentException>(() => { resolver.Handle(JsonSerializer.Serialize(data)); });
        }

        [Test]
        public void NoHandler()
        {
            var data = new Dictionary<string, object> {{"Handshake", new Handshake()}};
            Assert.Catch<ArgumentException>(() => { resolver.Handle(JsonSerializer.Serialize(data)); });
        }

        [Test]
        public void NodeNotReady()
        {
            var data = new Dictionary<string, object> {{"Message", new Message()}};
            nodeState.Ready = false;
            Assert.Catch<InvalidOperationException>(() => { resolver.Handle(JsonSerializer.Serialize(data)); });
        }

        [Test]
        public void InvalidModel()
        {
            var data = new Dictionary<string, object> {{"Message", new Message {Content = null}}};
            Assert.Catch<ValidationException>(() => { resolver.Handle(JsonSerializer.Serialize(data)); });
        }
        
        [Test]
        public void InvalidJson()
        {
            Assert.Catch<JsonException>(() => { resolver.Handle("not a json"); });
        }
    }
}