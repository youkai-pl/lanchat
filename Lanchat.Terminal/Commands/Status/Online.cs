using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

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
            Writer.WriteStatus(Resources.StatusChanged);
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}