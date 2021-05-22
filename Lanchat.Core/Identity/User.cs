using System.ComponentModel;
using Lanchat.Core.Network;

namespace Lanchat.Core.Identity
{
    internal class User : IUser, IInternalUser
    {
        private readonly INodeInternal node;
        private string nickname;
        private string previousNickname;
        private UserStatus userStatus;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        public User(INodeInternal node)
        {
            this.node = node;
        }

        /// <summary>
        ///     Node user nickname.
        /// </summary>
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
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        public string PreviousNickname => $"{previousNickname}#{ShortId}";

        /// <summary>
        ///     Short ID.
        /// </summary>
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
                OnPropertyChanged(nameof(UserStatus));
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}