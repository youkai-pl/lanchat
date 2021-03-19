using System;
using System.Collections.Generic;
using Lanchat.Core.Extensions;
using Lanchat.Core.Models;
using Lanchat.Core.NetworkIO;

namespace Lanchat.Core.System
{
    internal class NodeApiHandlers : IApiHandler
    {
        private readonly Node node;
        private readonly IConfig config;

        internal NodeApiHandlers(Node node, IConfig config)
        {
            this.node = node;
            this.config = config;
        }

        public IEnumerable<Type> HandledDataTypes { get; } = new[]
        {
            typeof(Goodbye),
            typeof(StatusUpdate),
            typeof(NicknameUpdate)
        };

        public void Handle(Type type, object data)
        {
            if (type == typeof(Goodbye))
            {
                node.NetworkElement.EnableReconnecting = false;
            }

            else if (type == typeof(StatusUpdate))
            {
                var status = (StatusUpdate) data;
                node.Status = status.NewStatus;
            }

            else if (type == typeof(NicknameUpdate))
            {
                var newNickname = (NicknameUpdate) data;
                node.Nickname = newNickname.NewNickname.Truncate(config.MaxNicknameLenght);
            }
        }
    }
}