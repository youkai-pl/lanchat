// Lanchat 2
// Bartłomiej Tota 2019

using lanchat.core.CryptographyLib;
using lanchat.core.NetworkLib;
using lanchat.PromptLib;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace lanchat
{
    internal class Program
    {
        public static IConfigurationRoot Config;

        private static void Main(string[] args)
        {
            // load or create config file
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("config.json", optional: false, reloadOnChange: true);
            Config = builder.Build();

            // show welcome screen
            Prompt.Welcome();

            // check nick
            Prompt.Out("Validating config");
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
            Prompt.Out("Validating RSA keys");
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

            // initialize prompt
            Prompt.Out("");
            new Thread(Prompt.Init).Start();

            // initialize network
            Client.Init(int.Parse(Config["bport"]),
                        Config["nickname"],
                        Cryptography.GetPublic());
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