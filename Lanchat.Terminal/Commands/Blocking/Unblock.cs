using System.Linq;
using System.Net;
using Lanchat.Core.Network;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;
        public int ContextArgsCount => ArgsCount;

        public void Execute(string[] args)
        {
            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (!correct)
            {
                Writer.WriteError(Resources.IncorrectValues);
                return;
            }

            if (Program.Config.BlockedAddresses.All(x => !Equals(x, parsedIp)))
            {
                Writer.WriteError(Resources.UserNotFound);
                return;
            }

            Program.Config.BlockedAddresses.Remove(parsedIp);
            Writer.WriteText(string.Format(Resources.Unblocked, parsedIp));
        }

        public void Execute(string[] args, INode node)
        {
            Execute(args);
        }
    }
}