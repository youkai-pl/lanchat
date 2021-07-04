using System;
using Lanchat.Core.Network;

namespace Lanchat.Core.Identity
{
    internal class User : IUser, IInternalUser
    {
        private readonly INodeInternal node;
        private string nickname;
        private string previousNickname;
        private UserStatus userStatus;

        public User(INodeInternal node)
        {
            this.node = node;
        }

        public string Nickname
        {
            get => $"{nickname}#{ShortId}";
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

        public string PreviousNickname => $"{previousNickname}#{ShortId}";
        public string ShortId => node.Id.GetHashCode().ToString().Substring(1, 4);

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