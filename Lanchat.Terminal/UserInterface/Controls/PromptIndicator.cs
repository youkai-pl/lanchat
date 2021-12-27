using ConsoleGUI.Controls;
using ConsoleGUI.UserDefined;
using Lanchat.Core.Identity;

namespace Lanchat.Terminal.UserInterface.Controls
{
    public class PromptIndicator : SimpleControl
    {
        private readonly TextBlock textBlock;

        public PromptIndicator()
        {
            textBlock = new()
            {
                Color = Theme.Foreground
            };

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
                UserStatus.Online => null,
                UserStatus.AwayFromKeyboard => "AFK",
                UserStatus.DoNotDisturb => "DND",
                _ => null
            };

            if (status == null)
            {
                Window.UiAction(() => textBlock.Text = "> ");
            }
            else
            {
                Window.UiAction(() => textBlock.Text = $"{status}> ");
            }
        }
    }
}