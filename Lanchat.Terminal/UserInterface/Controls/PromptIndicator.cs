using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class PromptIndicator : SimpleControl
    {
        private readonly TextBlock textBlock = new ();

        public PromptIndicator()
        {
            Program.Config.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName is "Nickname" or "UserStatus")
                {
                    UpdateText();
                }
            };
            
            UpdateText();
            Content = textBlock;
        }

        private void UpdateText()
        {
            textBlock.Text = $"[{Program.Config.Nickname} ({Program.Config.UserStatus})] ";
        }
    }
}