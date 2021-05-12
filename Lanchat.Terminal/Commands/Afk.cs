using Lanchat.Core.Chat;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Afk : ICommand
    {
        public string Alias => "afk";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.AwayFromKeyboard;
            Ui.BottomBar.Status.Text = "AFK";
        }
    }
}