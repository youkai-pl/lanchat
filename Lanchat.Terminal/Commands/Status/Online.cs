using Lanchat.Core.Identity;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands.Status
{
    public class Online : ICommand
    {
        public string Alias => "online";
        public int ArgsCount => 0;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.Online;
        }

        public void Execute(string[] args, INode context)
        {
            Execute(args);
        }
    }
}