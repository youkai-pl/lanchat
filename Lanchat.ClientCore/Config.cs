using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private static List<string> _blockedAddresses = new();
        private static bool _automaticConnecting = true;
        private static bool _useIPv6;
        private static string _language = "default";
        private int maxMessageLenght = 1500;
        private int maxNicknameLenght = 20;
        private Status status = Status.Online;
        private string filesDownloadDirectory = ConfigValues.GetDownloadsDirectory();
        
        [JsonIgnore] public bool Fresh { get; set; }

        [JsonIgnore]
        public List<IPAddress> BlockedAddresses
        {
            get => BlockedAddressesList.Select(IPAddress.Parse).ToList();
            set { BlockedAddressesList = value.Select(x => x.ToString()).ToList(); }
        }

        public List<string> BlockedAddressesList
        {
            get => _blockedAddresses;
            set { _blockedAddresses = value.Select(x => x.ToString()).ToList(); }
        }

        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged(nameof(Language));
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

        public int MaxNicknameLenght
        {
            get => maxNicknameLenght;
            set
            {
                maxNicknameLenght = value;
                OnPropertyChanged(nameof(MaxNicknameLenght));
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

        public int MaxMessageLenght
        {
            get => maxMessageLenght;
            set
            {
                maxMessageLenght = value;
                OnPropertyChanged(nameof(MaxMessageLenght));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void AddBlocked(IPAddress ipAddress)
        {
            var ipString = ipAddress.ToString();
            if (BlockedAddressesList.Contains(ipString)) return;
            BlockedAddressesList.Add(ipString);
            BlockedAddresses.Add(ipAddress);
            OnPropertyChanged(nameof(BlockedAddressesList));
        }

        public void RemoveBlocked(IPAddress ipAddress)
        {
            BlockedAddressesList.Remove(ipAddress.ToString());
            BlockedAddresses.Remove(ipAddress);
            OnPropertyChanged(nameof(BlockedAddressesList));
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}