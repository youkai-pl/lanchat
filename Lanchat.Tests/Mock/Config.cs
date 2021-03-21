using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Lanchat.Core;
using Lanchat.Core.Models;

namespace Lanchat.Tests.Mock
{
    public class ConfigMock : IConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Nickname { get; set; }
        public int ServerPort { get; set; }
        public int BroadcastPort { get; set; }
        public List<IPAddress> BlockedAddresses { get; set; }
        public Status Status { get; set; }
        public bool UseIPv6 { get; set; }
        public int MaxMessageLenght { get; set; }
        public int MaxNicknameLenght { get; set; }
        public bool AutomaticConnecting { get; set; }
        public string ReceivedFilesDirectory { get; set; }
    }
}