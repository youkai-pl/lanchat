// Lanchat 2
// Let's all love lain
using lanchat.Cli.PromptLib;
using Lanchat.Common.Cryptography;
using Lanchat.Common.Network;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace Lanchat.Cli.Program
{
    public static class Program
    {
        public static IConfigurationRoot Config;

        public static void Main()
        {
            // Load or create config file
            try
            {
                var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("config.json", optional: false, reloadOnChange: true);
                Config = builder.Build();
            }
            catch (Exception e)
            {
                CrashScreen(e);
            }

            // Show welcome screen
            Prompt.Welcome();

            // Validate config file
            Prompt.Out("Validating config");

            // Check nickname
            if (string.IsNullOrEmpty(Config["nickname"]))
            {
                // If nickname is blank create new with up to 20 characters
                string nick = Prompt.Query("Choose nickname:");
                while (nick.Length > 20)
                {
                    Prompt.Alert("Max 20 charcters");
                    nick = Prompt.Query("Choose nickname:");
                }
                EditConfig("nickname", nick);
            }

            // Try to load rsa settings
            try
            {
                Cryptography.Load(Config);
            }
            catch
            {
                Prompt.Out("Generating RSA keys");
                EditConfig("csp", Cryptography.Generate());
            }

            // Initialize prompt
            Prompt.Out("");
            new Thread(Prompt.Init).Start();

            // Initialize network
            Client.Init(int.Parse(Config["bport"]),
                        Config["nickname"],
                        Cryptography.GetPublic());
        }

        private static void CrashScreen(Exception e)
        {
            Prompt.Alert(e.Message);
            Prompt.Pause();
            Environment.Exit(1);
        }

        public static void EditConfig(string key, string value)
        {
            Config[key] = value;

            var newConfig = new
            {
                nickname = Config["nickname"],
                csp = Config["csp"],
                bport = Config["bport"]
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