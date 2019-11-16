// Lanchat 2
// Let's all love lain

using Lanchat.Cli.CommandsLib;
using Lanchat.Cli.ConfigLib;
using Lanchat.Cli.PromptLib;
using Lanchat.Common.Cryptography;
using Lanchat.Common.NetworkLib;
using System;
using System.Threading;

namespace Lanchat.Cli.Program
{
    public static class Program
    {
        public static void Main()
        {
            // Load or create config file
            Config.Load();

            // Show welcome screen
            Prompt.Welcome();

            // Validate config file
            Prompt.Out("Validating config");

            // Check nickname
            if (string.IsNullOrEmpty(Config.Get("nickname")))
            {
                // If nickname is blank create new with up to 20 characters
                string nick = Prompt.Query("Choose nickname:");
                while (nick.Length > 20)
                {
                    Prompt.Alert("Max 20 charcters");
                    nick = Prompt.Query("Choose nickname:");
                }
                Config.Edit("nickname", nick);
            }

            // Try to load rsa settings
            try
            {
                Cryptography.Load(Config.Get("csp"));
            }
            catch
            {
                Prompt.Out("Generating RSA keys");
                Config.Edit("csp", Cryptography.Generate());
            }

            // Initialize prompt
            Prompt Prompt1 = new Prompt();
            Prompt1.RecievedInput += OnRecievedInput;
            new Thread(Prompt1.Init).Start();

            // Initialize network
            Network.Init(int.Parse(Config.Get("port")),
                        Config.Get("nickname"),
                        Cryptography.GetPublic());
        }

        // Handle input
        private static void OnRecievedInput(string input, EventArgs e)
        {
            // Check is input command
            if (input.StartsWith("/"))
            {
                string command = input.Substring(1);
                Command.Execute(command);
            }

            // Or message
            else
            {
                Prompt.Out(input, null, Config.Get("nickname"));
                Network.SendAll(input);
            }
        }
    }
}