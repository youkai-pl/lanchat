using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lanchat.ClientCore
{
    internal static class ThemeLoader
    {
        private static JsonSerializerOptions JsonSerializerOptions => new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        private static readonly ThemeModelContext ThemeContext = new(JsonSerializerOptions);

        internal static ThemeModel Load(string themeName)
        {
            try
            {
                var json = File.ReadAllText(Path.Combine(Paths.ThemesDirectory, $"{themeName}.json"));
                return JsonSerializer.Deserialize(json, ThemeContext.ThemeModel);
            }
            catch (JsonException)
            {
                SaveDefaultTheme();
                return new ThemeModel();
            }
            catch (Exception e)
            {
                Storage.CatchFileSystemExceptionsStatic(e);
                SaveDefaultTheme();
                return new ThemeModel();
            }
        }

        private static void SaveDefaultTheme()
        {
            var json = JsonSerializer.Serialize(new ThemeModel(), ThemeContext.ThemeModel);
            Storage.SaveFile(json, Path.Combine(Paths.ThemesDirectory, "default.json"));
        }
    }
}