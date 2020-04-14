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
        private readonly Network network;

        public InputController(TextBox input, LogPanel log, Config config, Network network)
        {
            this.input = input;
            this.log = log;
            this.config = config;
            this.network = network;
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
                    network.Methods.SendAll(input.Text);
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
                    Nick.Execute(args, config, network);
                    break;

                case "help":
                    Help.Execute(args);
                    break;

                case "exit":
                    ExitLanchat.Execute();
                    break;

                case "connect":
                    Connect.Execute(args, network);
                    break;

                default:
                    break;
            }
        }
    }
}