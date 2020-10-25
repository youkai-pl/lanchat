using System;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Core;
using Lanchat.Terminal.Commands;

namespace Lanchat.Terminal.Ui
{
    public class InputController : IInputListener
    {
        private readonly Config config;
        private readonly TextBox input;
        private readonly LogPanel log;
        private readonly P2P network;

        public InputController(TextBox input, LogPanel log, Config config, P2P network)
        {
            this.input = input;
            this.log = log;
            this.config = config;
            this.network = network;
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Enter)
            {
                return;
            }
            
            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                if (input.Text.StartsWith("/", StringComparison.CurrentCulture))
                {
                    ExecuteCommand(input.Text.Split(' '));
                }
                else
                {
                    log.AddMessage(input.Text, config.Nickname);
                    network.BroadcastMessage(input.Text);
                }
            }

            input.Text = string.Empty;
            inputEvent.Handled = true;
        }

        private void ExecuteCommand(string[] args)
        {
            var command = args[0].Substring(1);
            args = args.Skip(1).ToArray();

            switch (command)
            {
                case "help":
                    Help.Execute(args);
                    break;

                case "exit":
                    Exit.Execute();
                    break;

                case "connect":
                    Connect.Execute(args, network);
                    break;
                
                case "disconnect":
                    Disconnect.Execute(args, network);
                    break;

                case "list":
                    List.Execute(network);
                    break;
                
                case "nick":
                    Nick.Execute(args, config);
                    break;
                
                case "block":
                    Block.Execute(args, config);
                    break;
                
                case "unblock":
                    Unblock.Execute(args, config);
                    break;
            }
        }
    }
}