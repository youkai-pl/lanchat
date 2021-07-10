using Lanchat.Core.Identity;

namespace Lanchat.Terminal.Commands.Status
{
    public class Afk : ICommand
    {
        public string Alias => "afk";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.AwayFromKeyboard;
        }
    }
}