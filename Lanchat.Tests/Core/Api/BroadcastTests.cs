using System.Collections.Generic;
using System.Linq;
using Lanchat.Core.Api;
using Lanchat.Core.Node;
using Lanchat.Tests.Mock;
using Lanchat.Tests.Mock.Models;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Api
{
    public class BroadcastTests
    {
        private Broadcast broadcast;
        private readonly List<INode> nodes = new();
        private readonly List<OutputMock> outputMocks = new();

        [OneTimeSetUp]
        public void Setup()
        {
            outputMocks.Add(new OutputMock());
            outputMocks.Add(new OutputMock());
            nodes.Add(new NodeMock(outputMocks.ElementAt(0)));
            nodes.Add(new NodeMock(outputMocks.ElementAt(1)));
            broadcast = new Broadcast(nodes);
        }

        [Test]
        public void SendMessage()
        {
            broadcast.SendMessage("test");
            Assert.IsTrue(outputMocks.All(x => x.LastOutput != null));
        }
        
        [Test]
        public void SendData()
        {
            broadcast.SendData(new Model());
            Assert.IsTrue(outputMocks.All(x => x.LastOutput != null));
        }
    }
}