using System;
using ConsoleGUI.Controls;
using ConsoleGUI.Input;
using Lanchat.Terminal.UserInterface.Views;

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
            
            if (tabPanel.CurrentTab is ChatView chatView)
            {
                chatView.Add(promptInput.Text);
                if (chatView.Broadcast)
                {
                    Program.Network.Broadcast.SendMessage(promptInput.Text);
                }
                else
                {
                    chatView.Node.Messaging.SendPrivateMessage(promptInput.Text);
                }
            }
            
            promptInput.Text = string.Empty;
            inputEvent.Handled = true;
        }
    }
}