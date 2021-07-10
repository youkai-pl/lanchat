using Lanchat.Core.Identity;

namespace Lanchat.Terminal.Commands.Status
{
    public class Dnd : ICommand
    {
        public string Alias => "dnd";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.DoNotDisturb;
        }
    }
}