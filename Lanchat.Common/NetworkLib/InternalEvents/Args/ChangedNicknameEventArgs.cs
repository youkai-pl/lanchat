using System;

namespace Lanchat.Common.NetworkLib.InternalEvents.Args
{
    /// <summary>
    /// Changed node nickname event.
    /// </summary>
    public class ChangedNicknameEventArgs : EventArgs
    {
        /// <summary>
        /// New nickname.
        /// </summary>
        public string NewNickname { get; set; }

        /// <summary>
        /// Old nickname.
        /// </summary>
        public string OldNickname { get; set; }
    }
}
