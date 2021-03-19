using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.ClientCore
{
    public abstract class DefaultConfig : IConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Nickname { get; set; } = NicknamesGenerator.GimmeNickname();
        public int ServerPort { get; set; } = 3645;
        public int BroadcastPort { get; set; } = 3646;
        public List<IPAddress> BlockedAddresses { get; set; } = new();
        public Status Status { get; set; } = Status.Online;
        public bool UseIPv6 { get; set; } = false;
        public int MaxMessageLenght { get; set; } = 1500;
        public int MaxNicknameLenght { get; set; } = 20;
        public bool AutomaticConnecting { get; set; } = true;
    }
}