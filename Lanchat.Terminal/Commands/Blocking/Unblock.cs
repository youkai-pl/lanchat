using System.Linq;
using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands.Blocking
{
    public class Unblock : ICommand
    {
        public string Alias => "unblock";
        public int ArgsCount => 1;

        public void Execute(string[] args)
        {
            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (!correct)
            {
                Window.Writer.WriteError(Resources._IncorrectValues);
                return;
            }

            if (Program.Config.BlockedAddresses.All(x => !Equals(x, parsedIp)))
            {
                Window.Writer.WriteError(Resources._UserNotFound);
                return;
            }

            Program.Config.BlockedAddresses.Remove(parsedIp);
            Window.Writer.WriteError(string.Format(Resources._Unblocked, parsedIp));
        }
    }
}