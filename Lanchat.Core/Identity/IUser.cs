using System;

namespace Lanchat.Core.Identity
{
    /// <summary>
    ///     Node user information readable for humans
    /// </summary>
    public interface IUser
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

        /// <summary>
        ///     User nickname changed.
        /// </summary>
        event EventHandler<string> NicknameUpdated;
        
        /// <summary>
        ///     User status changed.
        /// </summary>
        event EventHandler<UserStatus> StatusUpdated;
    }
}