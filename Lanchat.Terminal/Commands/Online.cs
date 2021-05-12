using Lanchat.Core.Chat;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Online : ICommand
    {
        public string Alias => "online";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.Online;
            Ui.BottomBar.Status.Text = "Online";
        }
    }
}