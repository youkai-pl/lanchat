using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.ClientCore
{
    public class Config : IConfig
    {
        private int port = 3645;
        private int broadcastPort = 3646;
        private string nickname = Environment.UserName;
        private bool automatic = true;
        private bool useIPv6;
        private string language = "default";
        private string filesDownloadDirectory = Storage.DownloadsPath;
        private Status status = Status.Online;
        private ObservableCollection<IPAddress> blockedAddresses = new();
        private ObservableCollection<IPAddress> savedAddresses = new();

        [JsonIgnore] public bool Fresh { get; set; }
        [JsonIgnore] public bool ConnectToSaved { get; set; } = true;
        [JsonIgnore] public bool NodesDetection { get; set; } = true;
        [JsonIgnore] public bool StartServer { get; set; } = true;

        public string Language
        {
            get => language;
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
        
        public ObservableCollection<IPAddress> BlockedAddresses
        {
            get => blockedAddresses;
            set
            {
                blockedAddresses = value;
                OnPropertyChanged(nameof(BlockedAddresses));
            }
        }

        public ObservableCollection<IPAddress> SavedAddresses
        {
            get => savedAddresses;
            set
            {
                savedAddresses = value;
                OnPropertyChanged(nameof(SavedAddresses));
            }
        }

        public Status Status
        {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int ServerPort
        {
            get => port;
            set
            {
                port = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }

        public int BroadcastPort
        {
            get => broadcastPort;
            set
            {
                broadcastPort = value;
                OnPropertyChanged(nameof(BroadcastPort));
            }
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

        public bool ConnectToReceivedList
        {
            get => automatic;
            set
            {
                automatic = value;
                OnPropertyChanged(nameof(ConnectToReceivedList));
            }
        }

        public string ReceivedFilesDirectory
        {
            get => filesDownloadDirectory;
            set
            {
                filesDownloadDirectory = value;
                OnPropertyChanged(nameof(ReceivedFilesDirectory));
            }
        }

        public bool UseIPv6
        {
            get => useIPv6;
            set
            {
                useIPv6 = value;
                OnPropertyChanged(nameof(UseIPv6));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}