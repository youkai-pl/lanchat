using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Identity;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class PromptIndicator : SimpleControl
    {
        private readonly TextBlock textBlock = new();

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
            var status = Program.Config.UserStatus switch
            {
                UserStatus.Online => "online",
                UserStatus.AwayFromKeyboard => "afk",
                UserStatus.DoNotDisturb => "dnd",
                _ => ""
            };
            Window.UiAction(() => textBlock.Text = $"[{Program.Config.Nickname} ({status})] ");
        }
    }
}