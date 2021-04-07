using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.Core.API
{
    internal class JsonReader
    {
        private readonly JsonSerializerOptions serializerOptions;

        public JsonReader()
        {
            serializerOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumConverter()
                }
            };
        }
        
        internal object DeserializeData(string jsonValue, Type type)
        {
            return JsonSerializer.Deserialize(jsonValue, type, serializerOptions);
        }

        internal Dictionary<string, JsonElement> DeserializeWrapper(string item)
        {
            return JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
        }
        
        internal T DeserializeKnownType<T>(string item)
        {
            var wrapper = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(item, serializerOptions);
            return JsonSerializer.Deserialize<T>(wrapper!.Values.First().ToString()!, serializerOptions);
        }
    }
}