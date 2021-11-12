using Lanchat.Core.Identity;
using Lanchat.Core.Network;

namespace Lanchat.Terminal.Commands.Status
{
    public class Online : ICommand
    {
        public string[] Aliases { get; } =
        {
            "online"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.Online;
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}