using Lanchat.Core.Identity;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Status
{
    public class Afk : ICommand
    {
        public string[] Aliases { get; } =
        {
            "afk"
        };
        public int ArgsCount => 0;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] _)
        {
            Program.Config.UserStatus = UserStatus.AwayFromKeyboard;
            Writer.WriteStatus(Resources.StatusChanged);
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}