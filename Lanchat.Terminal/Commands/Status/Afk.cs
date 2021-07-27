using Lanchat.Core.Identity;
using Lanchat.Core.Network;

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

        public void Execute(string[] args, INode context)
        {
            Execute(args);
        }
    }
}