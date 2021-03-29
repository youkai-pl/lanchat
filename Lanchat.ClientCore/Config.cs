using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.ClientCore
{
    public class Config : IConfig
    {
        private static int _port = 3645;
        private static int _broadcastPort = 3646;
        private static string _nickname = ConfigValues.GetNickname();
        private static bool _automaticConnecting = true;
        private static bool _useIPv6;
        private static string _language = "default";
        private string filesDownloadDirectory = ConfigValues.GetDownloadsDirectory();
        private Status status = Status.Online;
        private ObservableCollection<IPAddress> blockedAddresses = new();
        private ObservableCollection<IPAddress> savedAddresses = new();

        [JsonIgnore] public bool Fresh { get; set; }

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
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
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }

        public int BroadcastPort
        {
            get => _broadcastPort;
            set
            {
                _broadcastPort = value;
                OnPropertyChanged(nameof(BroadcastPort));
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                OnPropertyChanged(nameof(Nickname));
            }
        }

        public bool AutomaticConnecting
        {
            get => _automaticConnecting;
            set
            {
                _automaticConnecting = value;
                OnPropertyChanged(nameof(AutomaticConnecting));
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
            get => _useIPv6;
            set
            {
                _useIPv6 = value;
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