using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core.Config;
using Lanchat.Core.Identity;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Default IConfig implementation.
    /// </summary>
    /// <remarks>
    ///     Contains additional properties for Lanchat.Terminal.
    /// </remarks>
    public class Config : IConfig
    {
        private bool automatic = true;
        private int broadcastPort = 3646;
        private string filesDownloadDirectory = Paths.DownloadsDirectory;
        private string language = "default";
        private string nickname = Environment.UserName;
        private int port = 3645;
        private bool useIPv6;
        private UserStatus userStatus = UserStatus.Online;
        private string theme = "default";

        /// <summary>
        ///     True if config file could not be loaded and has just been generated.
        /// </summary>
        [JsonIgnore]
        public bool Fresh { get; set; }

        /// <summary>
        ///     UI language.
        /// </summary>
        public string Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        /// <inheritdoc />
        [JsonIgnore]
        public bool ConnectToSaved { get; set; } = true;

        /// <inheritdoc />
        [JsonIgnore]
        public bool NodesDetection { get; set; } = true;

        /// <inheritdoc />
        [JsonIgnore]
        public bool StartServer { get; set; } = true;

        /// <inheritdoc />
        public UserStatus UserStatus
        {
            get => userStatus;
            set
            {
                userStatus = value;
                OnPropertyChanged(nameof(UserStatus));
            }
        }

        /// <inheritdoc />
        public int ServerPort
        {
            get => port;
            set
            {
                port = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }

        /// <inheritdoc />
        public int BroadcastPort
        {
            get => broadcastPort;
            set
            {
                broadcastPort = value;
                OnPropertyChanged(nameof(BroadcastPort));
            }
        }

        /// <inheritdoc />
        public string Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        /// <inheritdoc />
        public bool ConnectToReceivedList
        {
            get => automatic;
            set
            {
                automatic = value;
                OnPropertyChanged(nameof(ConnectToReceivedList));
            }
        }

        /// <inheritdoc />
        public string ReceivedFilesDirectory
        {
            get => filesDownloadDirectory;
            set
            {
                filesDownloadDirectory = value;
                OnPropertyChanged(nameof(ReceivedFilesDirectory));
            }
        }

        /// <inheritdoc />
        public bool DebugMode { get; set; }

        /// <inheritdoc />
        public bool UseIPv6
        {
            get => useIPv6;
            set
            {
                useIPv6 = value;
                OnPropertyChanged(nameof(UseIPv6));
            }
        }

        /// <summary>
        ///     Lanchat.Terminal color scheme.
        /// </summary>
        public string Theme
        {
            get => theme;
            set
            {
                theme = value;
                OnPropertyChanged(nameof(Theme));
            }
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}