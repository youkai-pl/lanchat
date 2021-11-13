using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Terminal.Commands;
using Lanchat.Terminal.UserInterface.Views;

namespace Lanchat.Terminal.UserInterface
{
    public class InputController : IInputListener
    {
        private const int HistoryLimit = 50;
        private readonly CommandsManager commandsManager;
        private readonly List<string> history = new();
        private readonly TextBox promptInput;
        private int currentHistoryItem;

        public InputController(TextBox promptInput)
        {
            this.promptInput = promptInput;
            commandsManager = new CommandsManager();
        }

        public void OnInput(InputEvent inputEvent)
        {
            var key = inputEvent.Key.Key;

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    Window.UiAction(() => promptInput.Text = GetFromHistory(-1));
                    Window.UiAction(() => promptInput.Caret = promptInput.Text.Length);
                    inputEvent.Handled = true;
                    break;

                case ConsoleKey.DownArrow:
                    Window.UiAction(() => promptInput.Text = GetFromHistory(1));
                    Window.UiAction(() => promptInput.Caret = promptInput.Text.Length);
                    inputEvent.Handled = true;
                    break;

                case ConsoleKey.Enter:
                    ProcessInputText();
                    Window.UiAction(() => promptInput.Text = string.Empty);
                    inputEvent.Handled = true;
                    break;
            }
        }

        private void ProcessInputText()
        {
            if (string.IsNullOrWhiteSpace(promptInput.Text))
            {
                return;
            }

            if (Window.TabPanel.CurrentTab.Content is HomeView)
            {
                TabsManager.ShowMainChatView();
            }

            if (promptInput.Text.StartsWith("/", StringComparison.CurrentCulture))
            {
                commandsManager.ExecuteCommand(promptInput.Text.Split(' '));
                AddToHistory(promptInput.Text);
            }
            else if (Window.TabPanel.CurrentTab.Content is ChatView chatView)
            {
                SendMessage(chatView);
                AddToHistory(promptInput.Text);
            }
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

        private void AddToHistory(string input)
        {
            history.Add(input);
            currentHistoryItem = history.Count;
            if (history.Count <= HistoryLimit)
            {
                return;
            }

            history.Remove(history.First());
            currentHistoryItem--;
        }

        private string GetFromHistory(int direction)
        {
            currentHistoryItem += direction;
            var historyItem = history.ElementAtOrDefault(currentHistoryItem);

            if (historyItem != null)
            {
                return historyItem;
            }

            currentHistoryItem -= direction;
            return promptInput.Text;
        }
    }
}