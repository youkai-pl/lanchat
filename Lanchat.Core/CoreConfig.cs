using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Lanchat.Core.Models;

namespace Lanchat.Core
{
    /// <summary>
    ///     Lanchat configuration.
    /// </summary>
    public static class CoreConfig
    {
        private static string _nickname;
        private static Status _status = Status.Online;

        /// <summary>
        ///     User nickname.
        /// </summary>
        public static string Nickname
        {
            get => _nickname;
            set
            {
                if (_nickname == value) return;

                _nickname = value;
                NicknameChanged?.Invoke(null, value);
            }
        }

        public static Status Status
        {
            get => _status;
            set
            {
                if (_status == value) return;
                _status = value;
                StatusChanged?.Invoke(null, value);
            }
        }

        /// <summary>
        ///     Server port.
        /// </summary>
        public static int ServerPort { get; set; } = 3645;

        /// <summary>
        ///     Broadcast port.
        /// </summary>
        public static int BroadcastPort { get; set; } = 3646;

        /// <summary>
        ///     Max message lenght. Longer incoming messages will be trimmed.
        /// </summary>
        public static int MaxMessageLenght { get; set; } = 2000;

        /// <summary>
        ///     Max nickname lenght. Longer nicknames will be trimmed.
        /// </summary>
        public static int MaxNicknameLenght { get; set; } = 20;

        /// <summary>
        ///     Blocked IP addresses.
        /// </summary>
        public static List<IPAddress> BlockedAddresses { get; set; } = new();

        // Internal configurations
        internal static JsonSerializerOptions JsonSerializerOptions =>
            new()
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

        // Config events
        internal static event EventHandler<string> NicknameChanged;
        internal static event EventHandler<Status> StatusChanged;
    }
}