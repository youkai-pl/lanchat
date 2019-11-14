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

            // Check nick
            Prompt.Out("Validating config");

            if (string.IsNullOrEmpty(Config["nickname"]))
            {
                string nick = Prompt.Query("Choose nickname:");
                while (nick.Length > 20)
                {
                    Prompt.Alert("Max 20 charcters");
                    nick = Prompt.Query("Choose nickname:");
                }
                Config["nickname"] = nick;
                SaveConfig();
            }

            // Try to load rsa settings
            try
            {
                Cryptography.Load(Config);
            }
            catch
            {
                Prompt.Out("Generating RSA keys");
                Config["csp"] = Cryptography.Generate();
                SaveConfig();
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

        public static void SaveConfig()
        {
            var newConfig = new
            {
                nickname = Config["nickname"],
                csp = Config["csp"],
                mport = Config["mport"],
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