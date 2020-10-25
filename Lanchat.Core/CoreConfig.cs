using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core
{
    /// <summary>
    ///     Lanchat configuration.
    /// </summary>
    public static class CoreConfig
    {
        private static string _nickname;

        /// <summary>
        ///     User nickname.
        /// </summary>
        public static string Nickname
        {
            get => _nickname;
            set
            {
                if (_nickname == value)
                {
                    return;
                }

                _nickname = value;
                NicknameChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Server port.
        /// </summary>
        public static int ServerPort { get; set; } = 3645;

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
        public static List<IPAddress> BlockedAddresses { get; set; } = new List<IPAddress>();
        
        // Internal configurations
        internal static JsonSerializerOptions JsonSerializerOptions =>
            new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        
        // Config events
        internal static event EventHandler NicknameChanged;
    }
}