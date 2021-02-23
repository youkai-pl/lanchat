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

        internal NodeApiHandlers(Node node)
        {
            this.node = node;
        }

        public IEnumerable<DataTypes> HandledDataTypes { get; } = new[]
        {
            DataTypes.Goodbye,
            DataTypes.StatusUpdate,
            DataTypes.NicknameUpdate
        };

        public void Handle(DataTypes type, object data)
        {
            switch (type)
            {
                case DataTypes.Goodbye:
                    node.NetworkElement.EnableReconnecting = false;
                    break;

                case DataTypes.StatusUpdate:
                {
                    if (Enum.TryParse<Status>(data.ToString(), out var newStatus)) node.Status = newStatus;
                    break;
                }

                case DataTypes.NicknameUpdate:
                {
                    var newNickname = data.ToString();
                    node.Nickname = newNickname.Truncate(CoreConfig.MaxNicknameLenght);
                    break;
                }
            }
        }
    }
}