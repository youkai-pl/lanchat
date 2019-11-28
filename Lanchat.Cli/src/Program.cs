// Lanchat 2
// Let's all love lain

using Lanchat.Cli.CommandsLib;
using Lanchat.Cli.ConfigLib;
using Lanchat.Cli.PromptLib;
using Lanchat.Common.Cryptography;
using Lanchat.Common.NetworkLib;
using System;
using System.Diagnostics;
using System.Threading;

namespace Lanchat.Cli.Program
{
    public static class Program
    {
        private static Network network;

        public static void Main()
        {
            // Trace listener
            // Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));

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
            Prompt prompt = new Prompt();
            prompt.RecievedInput += OnRecievedInput;
            new Thread(prompt.Init).Start();

            // Initialize network
            network = new Network(int.Parse(Config.Get("port")), Config.Get("nickname"), Cryptography.GetPublic());
            network.RecievedMessage += OnRecievedMessage;
            network.NodeConnected += OnNodeConnected;
            network.NodeDisconnected += OnNodeDisconnected;
            network.Start();
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
                network.SendAll(input);
            }
        }

        // Handle message
        private static void OnRecievedMessage(object o, RecievedMessageEventArgs e)
        {
            // Console.WriteLine(e.Content + ": " + e.Nickname);
            Prompt.Out(e.Content, null, e.Nickname);
        }

        // Handle connect
        private static void OnNodeConnected(object o, NodeConnectionStatusEvent e)
        {
            // Console.WriteLine(e.Nickname + " connected");
            Prompt.Notice(e.Nickname + " connected");
        }

        // Handle disconnect
        private static void OnNodeDisconnected(object o, NodeConnectionStatusEvent e)
        {
            //Console.WriteLine(e.Nickname + " disconnected");
            Prompt.Notice(e.Nickname + " disconnected");
        }
    }
}