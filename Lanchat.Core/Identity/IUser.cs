using System.ComponentModel;

namespace Lanchat.Core.Identity
{
    /// <summary>
    ///     Node user information readable for humans
    /// </summary>
    public interface IUser : INotifyPropertyChanged
    {
        /// <summary>
        ///     Node user nickname.
        /// </summary>
        string Nickname { get; }

        /// <summary>
        ///     Nickname before last change.
        /// </summary>
        string PreviousNickname { get; }

        /// <summary>
        ///     Short ID.
        /// </summary>
        string ShortId { get; }

        /// <summary>
        ///     <see cref="Identity.UserStatus" />
        /// </summary>
        public UserStatus UserStatus { get; }
    }
}