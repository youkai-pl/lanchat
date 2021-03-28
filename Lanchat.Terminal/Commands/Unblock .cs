using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Unblock : ICommand
    {
        public string Alias { get; } = "unblock";
        public int ArgsCount { get; } = 1;

        public void Execute(string[] args)
        {
            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                Program.Config.RemoveBlocked(parsedIp);
                Ui.Log.Add(string.Format(Resources._Unblocked, parsedIp));
            }
            else
            {
                Ui.Log.AddError(Resources._IncorrectValues);
            }
        }
    }
}