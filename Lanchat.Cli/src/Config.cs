using System;
using System.IO;
using Lanchat.Cli.PromptLib;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Lanchat.Cli.ConfigLib
{
    class Config
    {
        private static IConfigurationRoot ConfigRoot;

        // Load config
        public static void Load()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", optional: false, reloadOnChange: true);
                ConfigRoot = builder.Build();
            }
            catch (Exception e)
            {
                Prompt.CrashScreen(e);
            }
        }

        // Get config value
        public static string Get(string key)
        {
            return ConfigRoot[key];
        }

        // Change config value
        public static void Edit(string key, string value)
        {
            ConfigRoot[key] = value;

            var newConfig = new
            {
                nickname = ConfigRoot["nickname"],
                csp = ConfigRoot["csp"],
                port = ConfigRoot["port"]
            };

            try
            {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(newConfig));
            }
            catch
            {
                Prompt.Alert("Config save error");
            }
        }
    }
}
