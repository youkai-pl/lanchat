using System;
using Lanchat.Core.Config;
using Lanchat.Core.Network;
using Lanchat.Core.TransportLayer;

namespace Lanchat.Core.Identity
{
    internal class User : IUser, IInternalUser
    {
        private readonly INodeInternal node;
        private readonly int nodeId;
        private string nickname;
        private string previousNickname;
        private UserStatus userStatus;

        public User(INodeInternal node, IHost host, INodesDatabase nodesDatabase)
        {
            nodeId = nodesDatabase.GetNodeInfo(host.Endpoint.Address).Id;
            this.node = node;
        }

        public string Nickname 
        {
            get => nickname;
            set
            {
                if (value == nickname)
                {
                    return;
                }

                previousNickname = nickname;
                nickname = value;
                if (node.Ready)
                {
                    NicknameUpdated?.Invoke(this, value);
                }
            }
        }
        
        public string NicknameWithId => $"{nickname}#{ShortId}";

        public string PreviousNickname => $"{previousNickname}#{ShortId}";
        public string ShortId => nodeId.ToString();

        public UserStatus UserStatus
        {
            get => userStatus;
            set
            {
                if (userStatus == value)
                {
                    return;
                }

                userStatus = value;
                StatusUpdated?.Invoke(this, value);
            }
        }

        public event EventHandler<string> NicknameUpdated;
        public event EventHandler<UserStatus> StatusUpdated;
    }
}