using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
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
        ///     Blocked IP addresses.
        /// </summary>
        List<IPAddress> BlockedAddresses { get; set; }

        /// <summary>
        ///     User status.
        /// </summary>
        Status Status { get; set; }

        /// <summary>
        ///     Use IPv6 instead IPv4.
        /// </summary>
        bool UseIPv6 { get; set; }

        /// <summary>
        ///     Max message lenght. Longer incoming messages will be trimmed.
        /// </summary>
        int MaxMessageLenght { get; set; }

        /// <summary>
        ///     Max nickname lenght. Longer nicknames will be trimmed.
        /// </summary>
        int MaxNicknameLenght { get; set; }

        /// <summary>
        ///     Enable automatic connecting to nodes from received list.
        /// </summary>
        bool AutomaticConnecting { get; set; }

        /// <summary>
        ///     Files download directory.
        /// </summary>
        string ReceivedFilesDirectory { get; set; }
    }
}