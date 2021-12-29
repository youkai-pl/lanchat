using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Status
{
    public class Dnd : ICommand
    {
        public string[] Aliases { get; } =
        {
            "dnd"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.DoNotDisturb;
            Writer.WriteStatus(Resources.StatusChanged);
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}