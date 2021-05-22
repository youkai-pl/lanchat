using System.ComponentModel;
using Lanchat.Core.Identity;

namespace Lanchat.Tests.Mock.Identity
{
    public class UserMock : IUser, IInternalUser
    {
        private string nickname;
        private UserStatus userStatus;
        public event PropertyChangedEventHandler PropertyChanged;

        public UserMock()
        {
            ShortId = "9999";
            Nickname = "test";
            PreviousNickname = "test";
        }

        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }
        
        public UserStatus UserStatus
        {
            get => userStatus;
            set
            {
                userStatus = value;
                OnPropertyChanged(nameof(UserStatus));
            }
        }

        public string PreviousNickname { get; }
        public string ShortId { get; }
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}