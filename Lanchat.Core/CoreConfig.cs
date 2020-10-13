using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core
{
    /// <summary>
    ///     Lanchat configuration.
    /// </summary>
    public static class CoreConfig
    {
        /// <summary>
        ///     User nickname.
        /// </summary>
        public static string Nickname { get; set; }

        /// <summary>
        ///    Server port. 
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
    }
}