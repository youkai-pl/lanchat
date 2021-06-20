using System;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;

namespace Lanchat.Terminal.UserInterface
{
    public class InputController : IInputListener
    {
        private readonly TextBox promptInput;
        private readonly TabPanel tabPanel;

        public InputController(TextBox promptInput, TabPanel tabPanel)
        {
            this.promptInput = promptInput;
            this.tabPanel = tabPanel;
        }
        
        public void OnInput(InputEvent inputEvent)
        {
            if (inputEvent.Key.Key != ConsoleKey.Enter)
            {
                return;
            }
            var chat = (ChatView)tabPanel.CurrentTab.Content;
            chat.Add(promptInput.Text);
            promptInput.Text = string.Empty;
            inputEvent.Handled = true;
        }
    }
}