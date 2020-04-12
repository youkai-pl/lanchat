using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Common.NetworkLib;
using System;

namespace Lanchat.Terminal.Ui
{
    public class InputController : IInputListener
    {
        private readonly TextBox input;
        private readonly LogPanel log;
        private readonly Config config;
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
                    log.Add(input.Text, Prompt.OutputType.Clear);
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
    }
}
