using System.Net;
using Lanchat.Terminal.Properties;
using Lanchat.Terminal.UserInterface;

namespace Lanchat.Terminal.Commands
{
    public class Unblock : ICommand
    {
        public string Alias { get; set; } = "unblock";
        public int ArgsCount { get; set; } = 1;

        public void Execute(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Ui.Log.Add(Resources.Help_unblock);
                return;
            }

            var correct = IPAddress.TryParse(args[0], out var parsedIp);
            if (correct)
            {
                Program.Config.RemoveBlocked(parsedIp);
                Ui.Log.Add(string.Format(Resources.Info_Unblocked, parsedIp));
            }
            else
            {
                Ui.Log.Add(Resources.Info_IncorrectValues);
            }
        }
    }
}