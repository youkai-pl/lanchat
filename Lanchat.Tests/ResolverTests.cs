using System;
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
            Assert.Catch<ArgumentException>(() => { resolver.Handle("test", "{}"); });
        }

        [Test]
        public void NoHandler()
        {
            Assert.Catch<ArgumentException>(() => { resolver.Handle("Handshake", "{}"); });
        }

        [Test]
        public void NodeNotReady()
        {
            var json = JsonSerializer.Serialize(new Message {Content = null});
            nodeState.Ready = false;
            Assert.Catch<InvalidOperationException>(() => { resolver.Handle("Message", json); });
        }

        [Test]
        public void InvalidData()
        {
            var json = JsonSerializer.Serialize(new Message {Content = null});
            Assert.Catch<ValidationException>(() => { resolver.Handle("Message", json); });
        }
    }
}