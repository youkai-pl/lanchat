using System;
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
                _nickname = value;
                NicknameChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Server port.
        /// </summary>
        public static int ServerPort { get; set; }

        // Internal configurations
        internal static JsonSerializerOptions JsonSerializerOptions =>
            new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };

        internal static event EventHandler NicknameChanged;
    }
}