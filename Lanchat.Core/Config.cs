using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core
{
    /// <summary>
    /// Lanchat configuration.
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// User nickname.
        /// </summary>
        public static string Nickname { get; set; }

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