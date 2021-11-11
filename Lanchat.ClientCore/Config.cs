using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core.Config;
using Lanchat.Core.Identity;

namespace Lanchat.ClientCore
{
    /// <summary>
    ///     Default IConfig implementation.
    /// </summary>
    public class Config : IConfig
    {
        private bool automatic = true;
        private ObservableCollection<IPAddress> blockedAddresses = new();
        private int broadcastPort = 3646;
        private string filesDownloadDirectory = Storage.DownloadsPath;
        private string language = "default";
        private string nickname = Environment.UserName;
        private int port = 3645;
        private bool useIPv6;
        private UserStatus userStatus = UserStatus.Online;

        /// <summary>
        ///     Config could not be loaded and has just been generated.
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
        public ObservableCollection<IPAddress> BlockedAddresses
        {
            get => blockedAddresses;
            set
            {
                blockedAddresses = value;
                OnPropertyChanged(nameof(BlockedAddresses));
            }
        }

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

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}