// Lanchat 2
// Bartłomiej Tota 2019

using lanchat.Crypto;
using lanchat.Network;
using lanchat.Terminal;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading;
using System.Text.Json;

namespace lanchat
{
    internal class Program
    {
        public static IConfigurationRoot Config;

        public static void SaveConfig()
        {
            var newConfig = new
            {
                nickname = Config["nickname"],
                csp = Config["csp"],
                mport = Config["mport"],
                bport = Config["bport"]
            };
            File.WriteAllText("config.json", JsonSerializer.Serialize(newConfig).ToString());
        }

        private static void Main(string[] args)
        {

            // load settings
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false, reloadOnChange: true);
            Config = builder.Build();

            // show welcome screen
            Prompt.Welcome();

            // check nick
            Prompt.Notice("Validating config");
            if (string.IsNullOrEmpty(Config["nickname"]))
            {
                string nick = Prompt.Query("Choose nickname: ");
                while (nick.Length > 20)
                {
                    Prompt.Alert("Max 20 charcters");
                    nick = Prompt.Query("Choose nickname: ");
                }
                Config["nickname"] = nick;
                SaveConfig();
            }

            // try to load rsa settings
            Prompt.Notice("Validating RSA keys");
            try
            {
                Cryptography.Load();
            }
            catch
            {
                Prompt.Notice("Generating RSA keys");
                Config["csp"] = Cryptography.Generate();
                SaveConfig();
            }

            // initialize prompt
            new Thread(Prompt.Init).Start();

            // initialize network
            Client.Init(int.Parse(Config["bport"]),
                        Config["nickname"],
                        Cryptography.GetPublic());
        }
    }
}