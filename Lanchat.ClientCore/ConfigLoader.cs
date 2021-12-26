using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    internal static class ConfigLoader
    {
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private static readonly ConfigContext ConfigContext = new(JsonSerializerOptions);

        internal static Config Load()
        {
            Config config;

            try
            {
                var json = File.ReadAllText(Paths.ConfigFile);
                config = JsonSerializer.Deserialize(json, ConfigContext.Config);
            }
            catch (JsonException)
            {
                config = new Config { Fresh = true };
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptionsStatic(e);
                config = new Config { Fresh = true };
            }

            SaveConfig(config);
            config.PropertyChanged += (_, _) => SaveConfig(config);
            return config;
        }

        private static void SaveConfig(Config config)
        {
            var json = JsonSerializer.Serialize(config, ConfigContext.Config);
            Storage.SaveFile(json, Paths.ConfigFile);
        }
    }
}