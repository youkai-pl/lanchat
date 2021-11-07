using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    [JsonSerializable(typeof(Config))]
    internal partial class JsonContext : JsonSerializerContext
    {
        
    }
}