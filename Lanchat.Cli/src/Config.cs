using Lanchat.Cli.PromptLib;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Lanchat.Cli.ConfigLib
{
    internal class Config
    {
        private static IConfigurationRoot ConfigRoot;

        // Load config
        public static void Load()
        {
            try
            {
                LoadConfigFile();

                // Rebuild and reaload config is it not valid
                if (!ValidateConfig())
                {
                    Trace.WriteLine("Not valid config");
                    RebuildConfig();
                    LoadConfigFile();
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    RebuildConfig();
                    LoadConfigFile();
                }
                else if (e is FormatException)
                {
                    Prompt.Alert("Corrupted config file. Hit any key to rebuild it or exit app and try fix it manually");
                    Console.ReadKey();
                    RebuildConfig();
                    LoadConfigFile();
                }
                else
                {
                    Console.WriteLine(e.GetType().ToString());
                    Prompt.CrashScreen(e);
                }
            }
        }

        // Load config from file
        private static void LoadConfigFile()
        {
            var builder = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("config.json", optional: false, reloadOnChange: true);
            ConfigRoot = builder.Build();
        }

        // Rebuild config
        private static void RebuildConfig()
        {
            var defaultConfig = new
            {
                csp = "",
                nickname = "",
                port = 4001
            };

            // Write default config to json file
            File.WriteAllText("config.json", JsonConvert.SerializeObject(defaultConfig));
            Trace.WriteLine("Config rebuilded");
        }

        // Validate config
        private static bool ValidateConfig()
        {
            return ConfigRoot["csp"] != null && ConfigRoot["nickname"] != null && ConfigRoot["port"] != null;
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