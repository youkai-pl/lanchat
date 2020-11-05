using System;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Terminal.Commands;

namespace Lanchat.Terminal.UserInterface
{
    public class InputController : IInputListener
    {
        private readonly TextBox input;

        public InputController(TextBox input)
        {
            this.input = input;
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
                    Ui.Log.AddMessage(input.Text, Program.Config.Nickname);
                    Program.Network.BroadcastMessage(input.Text);
                }
            }

            input.Text = string.Empty;
            inputEvent.Handled = true;
        }

        private static void ExecuteCommand(string[] args)
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
                    Connect.Execute(args);
                    break;

                case "disconnect":
                    Disconnect.Execute(args);
                    break;

                case "list":
                    List.Execute();
                    break;

                case "nick":
                    Nick.Execute(args);
                    break;

                case "block":
                    Block.Execute(args);
                    break;

                case "unblock":
                    Unblock.Execute(args);
                    break;
                
                case "blocked":
                    Blocked.Execute();
                    break;
                
                case "m":
                    PrivateMessage.Execute(args);
                    break;
            }
        }
    }
}