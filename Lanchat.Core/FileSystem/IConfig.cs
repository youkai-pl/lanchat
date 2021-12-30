using System.ComponentModel;
using Lanchat.Core.Identity;

namespace Lanchat.Core.Filesystem
{
    /// <summary>
    ///     Lanchat.Core configuration.
    /// </summary>
    public interface IConfig : INotifyPropertyChanged
    {
        /// <summary>
        ///     User nickname.
        /// </summary>
        string Nickname { get; set; }

        /// <summary>
        ///     Server port.
        /// </summary>
        int ServerPort { get; set; }

        /// <summary>
        ///     Broadcast port.
        /// </summary>
        int BroadcastPort { get; set; }

        /// <summary>
        ///     User status.
        /// </summary>
        UserStatus UserStatus { get; set; }

        /// <summary>
        ///     Try connecting with nodes from received list.
        /// </summary>
        bool ConnectToReceivedList { get; set; }

        /// <summary>
        ///     Try connecting with nodes from SavedAddresses.
        /// </summary>
        bool ConnectToSaved { get; set; }

        /// <summary>
        ///     Use UDP broadcasting to announce self presence and detect other nodes.
        /// </summary>
        bool NodesDetection { get; set; }

        /// <summary>
        ///     Disable only in debug mode.
        /// </summary>
        bool StartServer { get; set; }

        /// <summary>
        ///     Files download directory.
        /// </summary>
        string ReceivedFilesDirectory { get; set; }

        /// <summary>
        ///     Enable debug features.
        /// </summary>
        bool DebugMode { get; set; }
    }
}