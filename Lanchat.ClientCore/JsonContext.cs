using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    [JsonSerializable(typeof(Config))]
    internal partial class ConfigContext : JsonSerializerContext
    { }

    [JsonSerializable(typeof(List<NodeInfo>))]
    internal partial class NodesDatabaseContext : JsonSerializerContext
    { }
}