using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Common.NetworkLib;
using Lanchat.Terminal.Commands;
using System;
using System.Linq;

namespace Lanchat.Terminal.Ui
{
    public class InputController : IInputListener
    {
        private readonly Config config;
        private readonly TextBox input;
        private readonly LogPanel log;
        internal static Network Network;

        public InputController(TextBox input, LogPanel log, Config config)
        {
            this.input = input;
            this.log = log;
            this.config = config;
        }

        public void SetNetwork(Network network)
        {
            Network = network;
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent != null)
            {
                if (inputEvent.Key.Key != ConsoleKey.Enter) return;
            }
            else
            {
                throw new ArgumentNullException(nameof(inputEvent));
            }

            if (!string.IsNullOrWhiteSpace(input.Text))
            {
                if (input.Text.StartsWith("/", StringComparison.CurrentCulture))
                {
                    ExecuteCommand(input.Text.Split(' '));
                }
                else
                {
                    log.Add(input.Text, Prompt.OutputType.Message, config.Nickname);
                    Network.Methods.SendAll(input.Text);
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
                case "nick":
                    Nick.Execute(args, config, Network);
                    break;

                case "help":
                    Help.Execute(args);
                    break;

                case "exit":
                    ExitLanchat.Execute();
                    break;

                case "connect":
                    Connect.Execute(args, Network);
                    break;

                case "mute":
                    Mute.Execute(args, config, Network);
                    break;

                case "unmute":
                    Unmute.Execute(args, config, Network);
                    break;

                case "m":
                    Message.Execute(args, Network);
                    break;

                case "list":
                    List.Execute(Network);
                    break;

                default:
                    break;
            }
        }
    }
}