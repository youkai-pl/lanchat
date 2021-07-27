using Lanchat.Core.Identity;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands.Status
{
    public class Dnd : ICommand
    {
        public string Alias => "dnd";
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.DoNotDisturb;
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}