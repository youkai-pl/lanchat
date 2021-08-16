using System;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Terminal.Commands;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public class InputController : IInputListener
    {
        private readonly CommandsController commandsController;
        private readonly TextBox promptInput;

        public InputController(TextBox promptInput)
        {
            this.promptInput = promptInput;
            commandsController = new CommandsController();
        }

        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Enter)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(promptInput.Text))
            {
                if (Window.TabPanel.CurrentTab.Content is HomeView)
                {
                    TabsManager.ShowMainChatView();
                }

                if (promptInput.Text.StartsWith("/", StringComparison.CurrentCulture))
                {
                    commandsController.ExecuteCommand(promptInput.Text.Split(' '));
                }
                else if (Window.TabPanel.CurrentTab.Content is ChatView chatView)
                {
                    SendMessage(chatView);
                }
            }

            Window.UiAction(() => promptInput.Text = string.Empty);
            inputEvent.Handled = true;
        }

        private void SendMessage(ChatView chatView)
        {
            chatView.AddMessage(promptInput.Text, Program.Config.Nickname);
            if (chatView.Node == null)
            {
                Program.Network.Broadcast.SendMessage(promptInput.Text);
            }
            else
            {
                chatView.Node.Messaging.SendPrivateMessage(promptInput.Text);
            }
        }
    }
}